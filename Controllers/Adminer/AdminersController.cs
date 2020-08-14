﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System_Back_End.Models;
using System_Back_End.Repositories;
using System_Back_End.Services;
using System_Back_End.Services.Auth;

namespace System_Back_End.Controllers.Adminer
{
    [Route("api/admins")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    public class AdminersController : SharedAPIController
    {
        #region constructor and properties
        IAdminRepository _adminRepository;
        public AdminersController(UserManager<AppUser> userManager, IEmailSender emailSender, IAdminRepository adminRepository,
            AccountService accountService, IMapper mapper, TransactionService transactionService)
            : base(userManager, emailSender, accountService, mapper, transactionService)
        {
            _adminRepository = adminRepository;
        }
        #endregion

        #region ovveride methods
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            _accountService.SetCurrentContext(
                actionContext.HttpContext,
                new UrlHelper(actionContext)
                );
        }
        #endregion

        #region get
        [Authorize(Policy = "ViewAnySubAdminPolicy")]
        [HttpGet("{id}", Name = "GetAdminById")]
        public async Task<IActionResult> GetAdminByIdAsync(string id)
        {
            var _admin = await _adminRepository.GetAdminsShownModelById(id);
            if (_admin == null)
                return NotFound();
            return Ok(_admin);
        }

        [Authorize(Policy = "ViewAnySubAdminPolicy")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAdminsAsync(string adminType = null)
        {
            return Ok(await _adminRepository.GetAllAdminsShownModels(adminType).ToListAsync());
        }
        #endregion

        #region post
        [Authorize(Policy = "CanUpdateSubAdminPolicy")]
        [HttpPost]
        public async Task<IActionResult> AddNewAdmin(AddNewSubAdminModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
                return BadRequest();
            Admin admin = null;
            if (!await _accountService.AddSubNewAdmin(model, (_user, _admin) => {
                admin = _admin;
                user = _user;
            }))
                return BadRequest();
            return CreatedAtRoute(
                routeName: "GetAdminById",
                routeValues: new { id = user.Id }, admin);
        }
        #endregion

        #region delete
        [Authorize(Policy = "CanDeleteSubAdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(string id)
        {
            var adminToDelete = await _adminRepository.GetByIdAsync(id);
            if (adminToDelete.SuperAdminId == null)
                return BadRequest(Functions.MakeError("لايمكن حذف المسؤل الاساسى بشكل مباشر"));
            _adminRepository.Delete(adminToDelete);
            if (!await _adminRepository.SaveAsync())
                return StatusCode(500, Functions.MakeError("حدثت مشكلة اثناء معالجة طلبك"));
            return NoContent();
        }
        #endregion

        #region update
        [Authorize(Policy = "CanUpdateSubAdminPolicy")]
        [HttpPut("password/{id}")]
        public async Task<IActionResult> UpdateSubAdminPassword(string id,UpdateSubAdminPasswordModel model)
        {
            if (model == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var user =await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
           await _accountService.UpdateSubAdminPassword(user, model);
            return NoContent();
        }

        [Authorize(Policy = "CanUpdateSubAdminPolicy")]
        [HttpPut("username/{id}")]
        public async Task<IActionResult> UpdateSubAdminUserName(string id, UpdateSubAdminUserNameModel model)
        {
            if (model == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            if (user.UserName == model.NewUserName)
                return NoContent();
            var res=await _accountService.UpdateSubAdminUserName(user, model);
            if (!res.Succeeded)
                return BadRequest(Functions.MakeError(res.Errors.First().Description));
            return NoContent();
        }

        [Authorize(Policy = "CanUpdateSubAdminPolicy")]
        [HttpPut("phone/{id}")]
        public async Task<IActionResult> UpdateSubAdminPhoneNumber(string id, UpdateSubAdminPhoneNumberModel model)
        {
            if (model == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            if (user.PhoneNumber == model.PhoneNumber)
                return NoContent();
            var res = await _accountService.UpdateSubAdminPhoneNumber(user, model);
            if (!res.Succeeded)
                return BadRequest(Functions.MakeError(res.Errors.First().Description));
            return NoContent();
        }

        [Authorize(Policy = "CanUpdateSubAdminPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubAdmin(string id, UpdateSubAdminModel model)
        {
            if (model == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            if(! await _accountService.UpdateSubAdmin(user, model))
                return BadRequest(Functions.MakeError("لقد حدثت مشكلة فى قاعة البيانات اثناء التعديل"));
            return NoContent();
        }

        #endregion
    }
}