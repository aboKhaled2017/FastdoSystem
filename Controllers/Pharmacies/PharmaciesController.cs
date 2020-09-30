using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fastdo.backendsys.Models;
using Fastdo.backendsys.Repositories;
using Fastdo.backendsys.Services;
using Fastdo.backendsys.Services.Auth;
using Fastdo.Repositories.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fastdo.backendsys.Controllers
{
    [Route("api/pharmas")]
    [ApiController]
    [Authorize(Policy = "PharmacyPolicy")]
    public class PharmaciesController : SharedAPIController
    {
        #region constructor and properties
        public IStockRepository _stockRepository { get; private set; }
        public IPharmacyRepository _pharmacyRepository { get; private set; }

        public PharmaciesController(
            UserManager<AppUser> userManager, IEmailSender emailSender,
            AccountService accountService, IMapper mapper,
            IStockRepository stockRepository,
            IPharmacyRepository pharmacyRepository,
            TransactionService transactionService) : base(userManager, emailSender, accountService, mapper, transactionService)
        {
            _stockRepository = stockRepository;
            _pharmacyRepository = pharmacyRepository;
        }

        #endregion

        #region override methods from parent class
        [ApiExplorerSettings(IgnoreApi = true)]
        public override string Create_BMs_ResourceUri(ResourceParameters _params, ResourceUriType resourceUriType, string routeName)
        {
            var _cardParams = _params as StockSearchResourceParameters;
            switch (resourceUriType)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(routeName,
                    new StockSearchResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber - 1,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S,
                        AreaIds = _cardParams.AreaIds,
                        CityIds = _cardParams.CityIds 
                    });
                case ResourceUriType.NextPage:
                    return Url.Link(routeName,
                    new StockSearchResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber + 1,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S,
                        AreaIds = _cardParams.AreaIds,
                        CityIds = _cardParams.CityIds 
                    });
                default:
                    return Url.Link(routeName,
                    new StockSearchResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S,
                        AreaIds = _cardParams.AreaIds,
                        CityIds = _cardParams.CityIds 
                    });
            }
        }


        #endregion

        #region Get 
        [HttpGet("searchStks",Name = "GetPageOfSearchedStocks")]
        public async Task<IActionResult> SearchForStocks([FromQuery]StockSearchResourceParameters _params)
        {

            var BM_Cards = await _stockRepository.GetPageOfSearchedStocks(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<GetPageOfSearchedStocks, StockSearchResourceParameters>(
                BM_Cards, "GetPageOfSearchedStocks", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(BM_Cards);
        }
        [HttpGet("sentReqsStks")]
        public async Task<IActionResult> GetSentReqiestsToStocks()
        {
            return Ok(await _pharmacyRepository.GetSentRequestsToStocks());
        }
        [HttpGet("joinedStks")]
        public async Task<IActionResult> GetJoinedStocks()
        {
            return Ok(await _pharmacyRepository.GetUserJoinedStocks());
        }

        #endregion

        #region Put
        [HttpPut("stkRequests/{stockId}")]
        public async Task<IActionResult> MakeRequestToStock(string stockId)
        {
            if(await _stockRepository.MakeRequestToStock(stockId))
               return NoContent();
            return NotFound();
        }

        [HttpDelete("stkRequests/{stockId}")]
        public async Task<IActionResult> CancelRequestToStock(string stockId)
        {
            if (await _stockRepository.CancelRequestToStock(stockId))
                return NoContent();
            return NotFound();
        }
        #endregion

    }
}