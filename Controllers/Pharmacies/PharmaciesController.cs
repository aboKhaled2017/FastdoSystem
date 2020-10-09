﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fastdo.backendsys.Controllers.Pharmacies;
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
        public IStkDrugsRepository _stkDrugsRepository { get; private set; }
        public IPharmacyRepository _pharmacyRepository { get; private set; }

        public PharmaciesController(
            UserManager<AppUser> userManager, IEmailSender emailSender,
            AccountService accountService, IMapper mapper,
            IStockRepository stockRepository,
            IStkDrugsRepository stkDrugsRepository,
            IPharmacyRepository pharmacyRepository,
            TransactionService transactionService) : base(userManager, emailSender, accountService, mapper, transactionService)
        {
            _stockRepository = stockRepository;
            _pharmacyRepository = pharmacyRepository;
            _stkDrugsRepository = stkDrugsRepository;
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

        [ApiExplorerSettings(IgnoreApi = true)]
        public string CreateResourceUri_ForStkDrugs(ResourceParameters _params, ResourceUriType resourceUriType, string routeName)
        {
            var _cardParams = _params as StkDrugResourceParameters;
            switch (resourceUriType)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(routeName,
                    new StkDrugResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber - 1,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S
                    }); ;
                case ResourceUriType.NextPage:
                    return Url.Link(routeName,
                    new StkDrugResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber + 1,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S
                    });
                default:
                    return Url.Link(routeName,
                    new StkDrugResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S
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

        [HttpGet("stkdrugs", Name = "GetPageOfStockDrugsOfReportOfAllStocksFPH")]
        public async Task<IActionResult> GetStockDrugsOfReportForPharma([FromQuery]LzDrgResourceParameters _params)
        {

            var data = await _stkDrugsRepository.GetSearchedPageOfStockDrugsFPH(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<SearchGenralStkDrugModel_TargetPharma, LzDrgResourceParameters>(
                data, "GetPageOfStockDrugsOfReportOfAllStocksFPH", _params, CreateResourceUri_ForStkDrugs
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(data);
        }
        [HttpGet("stkdrugs/{stockId}", Name = "GetPageOfStockDrugsOfReportOfParticulatStockFPH")]
        public async Task<IActionResult> GetStockDrugsOfReportForPharma([FromQuery]LzDrgResourceParameters _params, [FromRoute] string stockId)
        {
            if (string.IsNullOrEmpty(stockId))
                return BadRequest();
            var data = await _stkDrugsRepository.GetSearchedPageOfStockDrugsFPH(stockId, _params);
            var paginationMetaData = new PaginationMetaDataGenerator<SearchStkDrugModel_TargetPharma, LzDrgResourceParameters>(
                data, "GetPageOfStockDrugsOfReportOfParticulatStockFPH", _params, CreateResourceUri_ForStkDrugs
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(data);
        }
        [HttpGet("stkdrugs/{stkDrugName}/stks")]
        public async Task<IActionResult> GetStocksOfSpecifiedStkDrugFPH(string stkDrugName)
        {
            if (string.IsNullOrEmpty(stkDrugName))
                return BadRequest();
            return Ok(await _stkDrugsRepository.GetStocksOfSpecifiedStkDrug(stkDrugName.Trim()));
        }

        #endregion

        #region Post

        [HttpPost("stkdrugpackage")]
        public async Task<IActionResult> MakeRequestForStkDrugsPackage([FromBody] IEnumerable<StkDrugsReqOfPharmaModel> stkDrugsList)
        {
            if (stkDrugsList.Count() == 0)
                return BadRequest();
            dynamic _error = null,_addedPackage=null;

            await _stkDrugsRepository.MakeRequestForStkDrugsPackage(stkDrugsList,package=> {
                _addedPackage = package;
            },error => {
                _error = error;
            });
            if (_error != null)
                return BadRequest(_error);

            return Ok(_addedPackage);
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

        [HttpPut("stkdrugpackage/{packageId}")]
        public async Task<IActionResult> UpdateRequestForStkDrugsPackage([FromRoute]Guid packageId,[FromBody] IEnumerable<StkDrugsReqOfPharmaModel> stkDrugsList)
        {
            if (stkDrugsList.Count() == 0 || packageId==null || packageId==Guid.Empty)
                return BadRequest();
            dynamic _error = null;

            await _stkDrugsRepository.UpdateRequestForStkDrugsPackage(packageId,stkDrugsList, error => {
                _error = error;
            });
            if (_error != null)
                return BadRequest(_error);

            return NoContent();
        }
        #endregion

        #region Delete

        [HttpDelete("stkdrugpackage/{packageId}")]
        public async Task<IActionResult> DeletePackageOfRequestedDrugs(Guid packageId)
        {
            if (packageId==null || packageId==Guid.Empty)
                return BadRequest();
            dynamic _error = null;

            await _stkDrugsRepository.DeleteRequestForStkDrugsPackage_FromStk(packageId, error => {
                _error = error;
            });
            if (_error != null)
                return BadRequest(_error);

            return NoContent();
        }

        #endregion

    }
}