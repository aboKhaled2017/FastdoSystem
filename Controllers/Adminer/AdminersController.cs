using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
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
using Fastdo.backendsys.Models;
using Fastdo.backendsys.Repositories;
using Fastdo.backendsys.Services;
using Fastdo.backendsys.Services.Auth;

namespace Fastdo.backendsys.Controllers.Adminer
{
    [Route("api/admins", Name = "AdminAccount")]
    [ApiController]
    [Authorize(Policy = "ControlOnAdministratorsPagePolicy")]
    public class AdminerCRUDController : MainAdminController
    {
        #region constructor and properties
        IAdminRepository _adminRepository;
        public AdminerCRUDController(UserManager<AppUser> userManager, IEmailSender emailSender, IAdminRepository adminRepository,
            AccountService accountService, IMapper mapper, TransactionService transactionService)
            : base(userManager, emailSender, accountService, mapper, transactionService)
        {
            _adminRepository = adminRepository;
        }
        #endregion

        #region ovveride methods
        [ApiExplorerSettings(IgnoreApi = true)]
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            _accountService.SetCurrentContext(
                actionContext.HttpContext,
                new UrlHelper(actionContext)
                );
        }
        #endregion

        #region override methods from parent class
        [ApiExplorerSettings(IgnoreApi = true)]
        public override string Create_BMs_ResourceUri(ResourceParameters _params, ResourceUriType resourceUriType, string routeName)
        {
            var _cardParams = _params as AdminersResourceParameters;
            switch (resourceUriType)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(routeName,
                    new AdminersResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber - 1,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S
                    });
                case ResourceUriType.NextPage:
                    return Url.Link(routeName,
                    new AdminersResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber + 1,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S
                    });
                default:
                    return Url.Link(routeName,
                    new AdminersResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S
                    });
            }
        }


        #endregion

        #region get
        [HttpGet("{id}", Name = "GetAdminById")]
        [Produces(typeof(ShowAdminModel))]
        public async Task<IActionResult> GetAdminByIdAsync(string id)
        {
            var _admin = await _adminRepository.GetAdminsShownModelById(id);
            if (_admin == null)
                return NotFound();
            return Ok(_admin);
        }

        [HttpGet("all",Name ="GEt_PageOFAdminers_ADM")]
        public async Task<ActionResult<IList<ShowAdminModel>>> GetAllAdminsAsync([FromQuery]AdminersResourceParameters _params)
        {
            var adminers=await _adminRepository.GET_PageOfAdminers_ShowModels_ADM(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<ShowAdminModel, AdminersResourceParameters>(
               adminers, "GEt_PageOFAdminers_ADM", _params, Create_BMs_ResourceUri
               ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(adminers);
        }
        #endregion

        #region post

        [HttpPost]
        public async Task<IActionResult> AddNewAdmin(AddNewSubAdminModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName.Trim());
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdminSync(string id)
        {
            var adminToDelete = await _adminRepository.GetByIdAsync(id);
            if (adminToDelete.SuperAdminId == null)
                return BadRequest(Functions.MakeError("لايمكن حذف المسؤل الاساسى بشكل مباشر"));
             _adminRepository.Remove(adminToDelete);
            if (!await _adminRepository.SaveAsync())
                return StatusCode(500, Functions.MakeError("حدثت مشكلة اثناء معالجة طلبك"));
            return NoContent();
        }
        #endregion

        #region update
        [HttpPut("password/{id}")]
        public async Task<IActionResult> UpdateSubAdminPasswordAsync(string id,UpdateSubAdminPasswordModel model)
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

        [HttpPut("username/{id}")]
        public async Task<IActionResult> UpdateSubAdminUserNameAsync(string id, UpdateSubAdminUserNameModel model)
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
            if (id == _userManager.GetUserId(User))
            {//it is the same user

                var currentUser =await _userManager.FindByIdAsync(id);
                var adminType = (await _userManager.GetClaimsAsync(currentUser))
                    .Single(c => c.Type == Variables.AdminClaimsTypes.AdminType).Value;
                var resToken =await _accountService.GetSigningInResponseModelForAdministrator(currentUser, adminType);
                return Ok(resToken);
            }
            return NoContent();
        }

        [HttpPut("phone/{id}")]
        public async Task<IActionResult> UpdateSubAdminPhoneNumberAsync(string id, UpdateSubAdminPhoneNumberModel model)
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
            if (id == _userManager.GetUserId(User))
            {//it is the same user

                var currentUser = await _userManager.FindByIdAsync(id);
                var adminType = (await _userManager.GetClaimsAsync(currentUser))
                    .Single(c => c.Type == Variables.AdminClaimsTypes.AdminType).Value;
                var resToken = await _accountService.GetSigningInResponseModelForAdministrator(currentUser, adminType);
                return Ok(resToken);
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubAdminAsync(string id, UpdateSubAdminModel model)
        {            
            if (model == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            if(! await _accountService.UpdateSubAdmin(user, model))
                return BadRequest(Functions.MakeError("لقد حدثت مشكلة فى قاعدة البيانات اثناء التعديل"));
            if (id == _userManager.GetUserId(User))
            {//it is the same user

                var currentUser = await _userManager.FindByIdAsync(id);
                var adminType = (await _userManager.GetClaimsAsync(currentUser))
                    .Single(c => c.Type == Variables.AdminClaimsTypes.AdminType).Value;
                var resToken = await _accountService.GetSigningInResponseModelForAdministrator(currentUser, adminType);
                return Ok(resToken);
            }
            return NoContent();
        }

        #endregion
    }
}