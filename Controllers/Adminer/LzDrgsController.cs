using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fastdo.API.Repositories;
using Fastdo.API.Services.Auth;
using Fastdo.Core;
using Fastdo.Core.Models;
using Fastdo.Core.Services;
using Fastdo.Core.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fastdo.API.Controllers.Adminer
{
    [Route("api/admins/drgs", Name = "AdminLzDrugs")]
    [ApiController]
    public class AdminLzDrgsController : MainAdminController
    {
        public AdminLzDrgsController(UserManager<AppUser> userManager, IEmailSender emailSender, IAccountService accountService, IMapper mapper, ITransactionService transactionService, IUnitOfWork unitOfWork) : base(userManager, emailSender, accountService, mapper, transactionService, unitOfWork)
        {
        }
        #region Constructors and properties

        #endregion

        #region get
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetLzDrugDetailsForAdmin([FromRoute] Guid id)
        {
            var drug =await _unitOfWork.LzDrugRepository.GEt_LzDrugDetails_For_ADM(id);
            if (drug == null)
                return NotFound();
            return Ok(drug);
        }
        #endregion
    }
}