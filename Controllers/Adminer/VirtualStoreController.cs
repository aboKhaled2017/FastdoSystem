using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fastdo.backendsys.Models;
using Fastdo.backendsys.Repositories;
using Fastdo.backendsys.Services.Auth;
using Fastdo.Repositories.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fastdo.backendsys.Controllers.Adminer
{
    [Route("api/admins/vstock", Name = "Admin_VStock")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    public class VirtualStoreController : SharedAPIController
    {
        #region Constructors and properties
        public ILzDrugRepository _lzDrugRepository { get; }
        public VirtualStoreController(AccountService accountService, IMapper mapper, UserManager<AppUser> userManager, ILzDrugRepository lzDrugRepository)
            : base(accountService, mapper, userManager)
        {
            _lzDrugRepository = lzDrugRepository;
        }

        #endregion

        #region get
        [HttpGet(Name = "GET_PageOf_VStock_LzDrugs")]
        public async Task<IActionResult> GETPageOfVStockDrugsForAdmin([FromQuery]LzDrgResourceParameters _params)
        {
            var allDrugsData = await _lzDrugRepository.GET_PageOf_VStock_LzDrgs(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<Show_VStock_LzDrg_ADM_Model, LzDrgResourceParameters>(
                allDrugsData, "GET_PageOf_VStock_LzDrugs", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(allDrugsData);
        }
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GETVStockDrugDetailsForAdmin([FromRoute] Guid id)
        {
            var drugDetails = await _lzDrugRepository.GEt_LzDrugDetails_For_ADM(id);
            if (drugDetails == null)
                return NotFound();
            return Ok(drugDetails);
        }
        #endregion
    }
}