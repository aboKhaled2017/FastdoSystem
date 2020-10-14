using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fastdo.backendsys.Repositories;
using Fastdo.backendsys.Services.Auth;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fastdo.backendsys.Controllers.Adminer
{
    [Route("api/admins/drgs", Name = "AdminLzDrugs")]
    [ApiController]
    public class AdminLzDrgsController : MainAdminController
    {
        #region Constructors and properties
        public ILzDrugRepository _lzDrugRepository { get; }
        public AdminLzDrgsController(AccountService accountService, IMapper mapper, UserManager<AppUser> userManager,ILzDrugRepository lzDrugRepository) 
            : base(accountService, mapper, userManager)
        {
            _lzDrugRepository = lzDrugRepository;
        }

        #endregion

        #region get
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetLzDrugDetailsForAdmin([FromRoute] Guid id)
        {
            var drug =await _lzDrugRepository.GEt_LzDrugDetails_For_ADM(id);
            if (drug == null)
                return NotFound();
            return Ok(drug);
        }
        #endregion
    }
}