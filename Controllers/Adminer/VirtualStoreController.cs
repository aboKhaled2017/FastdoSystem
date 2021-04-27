using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fastdo.Core.ViewModels;
using Fastdo.API.Repositories;
using Fastdo.API.Services.Auth;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Fastdo.Core.Services.Auth;
using Fastdo.Core;
using Microsoft.AspNetCore.Identity.UI.Services;
using Fastdo.Core.Services;

namespace Fastdo.API.Controllers.Adminer
{
    [Route("api/admins/vstock", Name = "Admin_VStock")]
    [ApiController]
    [Authorize(Policy = "ControlOnVStockPagePolicy")]
    public class VirtualStoreController : MainAdminController
    {
        public VirtualStoreController(UserManager<AppUser> userManager, IEmailSender emailSender, IAccountService accountService, IMapper mapper, ITransactionService transactionService, IUnitOfWork unitOfWork) : base(userManager, emailSender, accountService, mapper, transactionService, unitOfWork)
        {
        }

        #region override methods from parent class
        [ApiExplorerSettings(IgnoreApi = true)]
        public override string Create_BMs_ResourceUri(IResourceParameters _params, ResourceUriType resourceUriType, string routeName)
        {
            var _cardParams = _params as LzDrgResourceParameters;
            switch (resourceUriType)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(routeName,
                    new LzDrgResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber - 1,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S
                    });
                case ResourceUriType.NextPage:
                    return Url.Link(routeName,
                    new LzDrgResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber + 1,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S
                    });
                default:
                    return Url.Link(routeName,
                    new LzDrgResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S
                    });
            }
        }


        #endregion

        #region get
        [HttpGet(Name = "GET_PageOf_VStock_LzDrugs")]
        public async Task<IActionResult> GETPageOfVStockDrugsForAdmin([FromQuery]LzDrgResourceParameters _params)
        {
            var allDrugsData = await _unitOfWork.LzDrugRepository.GET_PageOf_VStock_LzDrgs(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<Show_VStock_LzDrg_ADM_Model, LzDrgResourceParameters>(
                allDrugsData, "GET_PageOf_VStock_LzDrugs", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(allDrugsData);
        }
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GETVStockDrugDetailsForAdmin([FromRoute] Guid id)
        {
            var drugDetails = await _unitOfWork.LzDrugRepository.GEt_LzDrugDetails_For_ADM(id);
            if (drugDetails == null)
                return NotFound();
            return Ok(drugDetails);
        }
        #endregion
    }
}