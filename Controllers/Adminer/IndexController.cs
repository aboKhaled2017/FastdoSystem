using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Fastdo.backendsys.Repositories;
using Fastdo.backendsys.Services;
using Fastdo.backendsys.Services.Auth;

namespace Fastdo.backendsys.Controllers.Adminer
{
    [Route("api/admin", Name = "Admin")]
    [ApiController]
    public class AdminIndexController : MainAdminController
    {

        #region constructor and properties
        IAdminRepository _adminRepository;
        public AdminIndexController(UserManager<AppUser> userManager, IEmailSender emailSender, IAdminRepository  adminRepository,
            AccountService accountService, IMapper mapper, TransactionService transactionService) 
            : base(userManager, emailSender, accountService, mapper, transactionService)
        {
            _adminRepository = adminRepository;
        }
        #endregion

        #region get
        [HttpGet("statis")]
        public async Task<IActionResult> GetGeneralStatisticsForAdmin()
        {
            return Ok(await _adminRepository.GetGeneralStatisOfSystem());
        }
        #endregion

    }
}