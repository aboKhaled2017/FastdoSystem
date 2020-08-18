using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Repositories.Models;
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
    [Route("api/admin")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    public class IndexController : SharedAPIController
    {
        #region constructor and properties
        IAdminRepository _adminRepository;
        public IndexController(UserManager<AppUser> userManager, IEmailSender emailSender, IAdminRepository  adminRepository,
            AccountService accountService, IMapper mapper, TransactionService transactionService) 
            : base(userManager, emailSender, accountService, mapper, transactionService)
        {
            _adminRepository = adminRepository;
        }
        #endregion

        #region get
        [HttpGet("statis")]
        public async Task<IActionResult> GetGeneralStatistics()
        {
            return Ok(await _adminRepository.GetGeneralStatisOfSystem());
        }
        #endregion

    }

}