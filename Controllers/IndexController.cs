using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fastdo.API.Repositories;
using Fastdo.API.Services;
using Fastdo.API.Services.Auth;
using Fastdo.Core.Services;
using Fastdo.Core;
using Fastdo.Core.Services.Auth;

namespace Fastdo.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class IndexController : SharedAPIController
    {
        public IndexController(UserManager<AppUser> userManager, IEmailSender emailSender, IAccountService accountService, IMapper mapper, ITransactionService transactionService, IUnitOfWork unitOfWork) : base(userManager, emailSender, accountService, mapper, transactionService, unitOfWork)
        {
        }


        #region get
        [HttpGet("areas/all")]
        public ActionResult getAllAreas()
        {

            return Ok(_unitOfWork.AreaRepository.GetAll().Select(a=>new { 
              a.Id,
              a.Name,
              a.SuperAreaId
            }));
        }
        [HttpGet("users/all")]
        public async Task<ActionResult> getAllUsers()
        {
            var p = Properties.EmailConfig;
            var stockUsers = (await _userManager
                .GetUsersInRoleAsync(Variables.stocker))
                .Select(u => new
                {
                    u.Id,
                    u.Email,
                    u.PhoneNumber,
                    u.EmailConfirmed
                }).ToList();
            var pharmacyUsers = (await _userManager
               .GetUsersInRoleAsync(Variables.pharmacier))
               .Select(u => new
               {
                   u.Id,
                   u.Email,
                   u.PhoneNumber,
                   u.EmailConfirmed
               }).ToList();
            return Ok(new { 
            stocks=stockUsers,
            pharmacies=pharmacyUsers
            });
        }
        [HttpGet("users/{userId}")]
        public async Task<ActionResult> getUserById(string userId)
        {

            var user = await _userManager.Users               
                .Select(u => new
                {
                    u.Id,
                    u.Email,
                    u.PhoneNumber,
                    u.EmailConfirmed
                }).FirstOrDefaultAsync(u=>u.Id==userId);
            return Ok(user);
        }

        [HttpGet("pharmacies/all")]
        public async Task<ActionResult> getAllPharmacies()
        {
            var data = _unitOfWork.PharmacyRepository.GetAll().Select(p => new
            {
                p.Id,
                email = p.User.Email,
                drugsNum = p.LzDrugs.Count,
                drugsNames = p.LzDrugs.Select(d =>new {d.Id,d.Name})
            });
            return Ok(await data.ToListAsync());
                   
        }
        #endregion

        #region methods for test
        [HttpGet("test")]
        public HttpResponseMessage test(ConfirmEmailModel model)
        {
            return new HttpResponseMessage(HttpStatusCode.Created);
        }
        #endregion
    }
}