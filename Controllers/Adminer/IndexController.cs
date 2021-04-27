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
using Fastdo.API.Repositories;
using Fastdo.API.Services;
using Fastdo.API.Services.Auth;
using Fastdo.Core.Services.Auth;
using Fastdo.Core.Services;
using Fastdo.Core;

namespace Fastdo.API.Controllers.Adminer
{
    [Route("api/admin", Name = "Admin")]
    [ApiController]
    public class AdminIndexController : MainAdminController
    {
        public AdminIndexController(UserManager<AppUser> userManager, IEmailSender emailSender, IAccountService accountService, IMapper mapper, ITransactionService transactionService, IUnitOfWork unitOfWork) : base(userManager, emailSender, accountService, mapper, transactionService, unitOfWork)
        {
        }


        #region get
        [HttpGet("statis")]
        public async Task<IActionResult> GetGeneralStatisticsForAdmin()
        {
            return Ok(await _unitOfWork.AdminRepository.GetGeneralStatisOfSystem());
        }
        #endregion

    }
}