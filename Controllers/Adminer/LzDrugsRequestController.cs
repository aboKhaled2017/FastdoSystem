using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fastdo.backendsys.Models;
using Fastdo.backendsys.Repositories;
using Fastdo.backendsys.Services.Auth;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fastdo.backendsys.Controllers.Adminer
{
    [Route("api/admins/drgsReq",Name = "AdminLzDrugRequests")]
    [ApiController]
    [Authorize(Policy = "ControlOnDrugsRequestsPagePolicy")]
    public class AdminLzDrugsRequestController : MainAdminController
    {
        #region constructor and properties
        private readonly ILzDrgRequestsRepository _lzDrgRequestsRepository;
        private readonly ILzDrugRepository _lzDrugRepository;

        public AdminLzDrugsRequestController(AccountService accountService, IMapper mapper, UserManager<AppUser> userManager,
            ILzDrgRequestsRepository lzDrgRequestsRepository,ILzDrugRepository lzDrugRepository) 
            : base(accountService, mapper, userManager)
        {
            _lzDrgRequestsRepository = lzDrgRequestsRepository;
            _lzDrugRepository = lzDrugRepository;
        }
        #endregion

        #region override methods from parent class
        [ApiExplorerSettings(IgnoreApi = true)]
        public override string Create_BMs_ResourceUri(ResourceParameters _params, ResourceUriType resourceUriType, string routeName)
        {
            var _cardParams = _params as LzDrgReqResourceParameters;
            switch (resourceUriType)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(routeName,
                    new LzDrgReqResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber - 1,
                        PageSize = _cardParams.PageSize,
                        Status = _cardParams.Status,
                        Seen = _cardParams.Seen
                    }); ;
                case ResourceUriType.NextPage:
                    return Url.Link(routeName,
                    new LzDrgReqResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber + 1,
                        PageSize = _cardParams.PageSize,
                        Status = _cardParams.Status,
                        Seen = _cardParams.Seen
                    });
                default:
                    return Url.Link(routeName,
                    new LzDrgReqResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber,
                        PageSize = _cardParams.PageSize,
                        Status = _cardParams.Status,
                        Seen = _cardParams.Seen
                    });
            }
        }
        #endregion

        #region  get
        [HttpGet(Name = "GET_PageOf_LzDrgsRequests")]
        public async Task<IActionResult> GetPageOfLzDrgsRequests([FromQuery]LzDrgReqResourceParameters _params)
        {
            var requests = await _lzDrgRequestsRepository.GET_PageOf_LzDrgsRequests(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<Show_LzDrgsReq_ADM_Model, LzDrgReqResourceParameters>(
                requests, "GET_PageOf_LzDrgsRequests", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(requests);
        }
        #endregion
    }
}