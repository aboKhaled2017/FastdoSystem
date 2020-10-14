using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Fastdo.backendsys.Models;
using Fastdo.backendsys.Repositories;
using Fastdo.backendsys.Services;
using Fastdo.backendsys.Services.Auth;

namespace Fastdo.backendsys.Controllers
{
    [Route("api/lzdrug/search")]
    [ApiController]
    [Authorize(Policy = "PharmacyPolicy")]
    public class LzDrugSearchController : SharedAPIController
    {
        #region constructor and properties
        public ILzDrg_Search_Repository _lzDrg_Search_Repository { get; }
        public IpropertyMappingService _propertyMappingService { get; }

        public LzDrugSearchController(
            AccountService accountService, IMapper mapper,
            ILzDrg_Search_Repository lzDrg_Search_Repository,
            IpropertyMappingService propertyMappingService,
            UserManager<AppUser> userManager) 
            : base(accountService, mapper, userManager)
        {
            _lzDrg_Search_Repository = lzDrg_Search_Repository;
            _propertyMappingService = propertyMappingService;
        }
        #endregion

        #region override methods from parent class
        [ApiExplorerSettings(IgnoreApi = true)]
        public override string Create_BMs_ResourceUri(ResourceParameters _params, ResourceUriType resourceUriType, string routeName)
        {
            var _cardParams = _params as LzDrg_Card_Info_BM_ResourceParameters;
            switch (resourceUriType)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(routeName,
                    new LzDrg_Card_Info_BM_ResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber - 1,
                        PageSize = _cardParams.PageSize,
                        S= _cardParams.S,
                        PhramId=_cardParams.PhramId,
                        AreaIds=_cardParams.AreaIds,
                        CityIds=_cardParams.CityIds,
                        ValidBefore=_cardParams.ValidBefore,
                        OrderBy=_cardParams.OrderBy
                    });
                case ResourceUriType.NextPage:
                    return Url.Link(routeName,
                    new LzDrg_Card_Info_BM_ResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber + 1,
                        PageSize = _cardParams.PageSize,
                        S=_cardParams.S,
                        PhramId = _cardParams.PhramId,
                        AreaIds = _cardParams.AreaIds,
                        CityIds = _cardParams.CityIds,
                        ValidBefore = _cardParams.ValidBefore,
                        OrderBy = _cardParams.OrderBy
                    });
                default:
                    return Url.Link(routeName,
                    new LzDrg_Card_Info_BM_ResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber,
                        PageSize = _cardParams.PageSize,
                        S=_cardParams.S,
                        PhramId = _cardParams.PhramId,
                        AreaIds = _cardParams.AreaIds,
                        CityIds = _cardParams.CityIds,
                        ValidBefore = _cardParams.ValidBefore,
                        OrderBy = _cardParams.OrderBy
                    });
            }
        }


        #endregion

        #region Get All LzDrugs Cards For Search
        [HttpGet(Name ="GetAll_LzDrug_CardInfo_BMs")]
        public async Task<IActionResult> GetPageOfSearchedLzDrugs([FromQuery]LzDrg_Card_Info_BM_ResourceParameters _params)
        {
            if (!_propertyMappingService.validMappingExistsFor<LzDrugCard_Info_BM, LzDrug>(_params.OrderBy))
                return BadRequest();
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

        #endregion

    }
}