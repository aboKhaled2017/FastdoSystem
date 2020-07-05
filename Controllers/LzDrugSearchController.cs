using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System_Back_End.Models;
using System_Back_End.Repositories;
using System_Back_End.Services.Auth;

namespace System_Back_End.Controllers
{
    [Route("api/lzdrug/search")]
    [ApiController]
    [Authorize(Policy = "PharmacyPolicy")]
    public class LzDrugSearchController : SharedAPIController
    {
        public ILzDrg_Search_Repository _lzDrg_Search_Repository { get; }
        public LzDrugSearchController(
            AccountService accountService, IMapper mapper,
            ILzDrg_Search_Repository lzDrg_Search_Repository,
            UserManager<AppUser> userManager) 
            : base(accountService, mapper, userManager)
        {
            _lzDrg_Search_Repository = lzDrg_Search_Repository;
        }

        [HttpGet(Name ="GetAll_LzDrug_CardInfo_BMs")]
        public async Task<IActionResult> GetAll([FromQuery]LzDrg_Card_Info_BM_ResourceParameters _params)
        {
            var BM_Cards =await _lzDrg_Search_Repository.Get_All_LzDrug_Cards_BMs(_params);
            BM_Cards.ForEach(BM_Card =>
            {
                BM_Card.RequestUrl = Url.Link("Add_LzDrug_Request_For_User",new { drugId =BM_Card.Id});
                BM_Card.Status = BM_Card.IsMadeRequest ? BM_Card.Status : null;
            });
            var paginationMetaData = new PaginationMetaDataGenerator<LzDrugCard_Info_BM, LzDrg_Card_Info_BM_ResourceParameters>(
                BM_Cards, "GetAll_LzDrug_CardInfo_BMs", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(BM_Cards);
        }
        
    }
}