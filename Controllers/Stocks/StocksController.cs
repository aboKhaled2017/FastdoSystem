﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fastdo.backendsys.Controllers.Stocks.Models;
using Fastdo.backendsys.Models;
using Fastdo.backendsys.Repositories;
using Fastdo.backendsys.Services;
using Fastdo.backendsys.Services.Auth;
using Fastdo.Repositories.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Fastdo.backendsys.Controllers.Stocks
{
    [Route("api/stk")]
    [ApiController]
    [Authorize(Policy = "StockPolicy")]
    public class StocksController : SharedAPIController
    {

        #region constructors and properties
        public StkDrugsReportFromExcelService _stkDrugsReportFromExcelService { get; private set; }
        public TransactionService _transactionService { get; private set; }
        public IStockRepository _stockRepository { get; private set; }
        public IStkDrugsRepository _stkDrugsRepository { get; private set; }

        public StocksController(
            AccountService accountService, IMapper mapper,
            StkDrugsReportFromExcelService StkDrugsReportFromExcelService,
            IStkDrugsRepository stkDrugsRepository,
            IStockRepository stockRepository,
            TransactionService transactionService,
            UserManager<AppUser> userManager) 
            : base(accountService, mapper, userManager)
        {
            _stkDrugsReportFromExcelService = StkDrugsReportFromExcelService;
            _stkDrugsRepository = stkDrugsRepository;
            _transactionService = transactionService;
            _stockRepository = stockRepository;
    }


        #endregion

        #region override methods from parent class
        [ApiExplorerSettings(IgnoreApi = true)]
        public override string Create_BMs_ResourceUri(ResourceParameters _params, ResourceUriType resourceUriType, string routeName)
        {           
            var _requestsParams = _params as PharmaReqsResourceParameters;
            switch (resourceUriType)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(routeName,
                    new PharmaReqsResourceParameters
                    {
                        PageNumber = _requestsParams.PageNumber - 1,
                        PageSize = _requestsParams.PageSize,
                        S = _requestsParams.S ,
                        PharmaClass=_requestsParams.PharmaClass,
                        Status=_requestsParams.Status
                    }); ;
                case ResourceUriType.NextPage:
                    return Url.Link(routeName,
                    new PharmaReqsResourceParameters
                    {
                        PageNumber = _requestsParams.PageNumber + 1,
                        PageSize = _requestsParams.PageSize,
                        S = _requestsParams.S,
                        PharmaClass = _requestsParams.PharmaClass,
                        Status = _requestsParams.Status
                    });
                default:
                    return Url.Link(routeName,
                    new PharmaReqsResourceParameters
                    {
                        PageNumber = _requestsParams.PageNumber,
                        PageSize = _requestsParams.PageSize,
                        S = _requestsParams.S,
                        PharmaClass = _requestsParams.PharmaClass,
                        Status = _requestsParams.Status
                    });
            }
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public string CreateResourceUriForPhamaReques(ResourceParameters _params, ResourceUriType resourceUriType, string routeName)
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
                        }); ;
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

        #region update the stock report
        [HttpPut("prods")]
        public async Task<IActionResult> UpdateDrugsReporst([FromForm] StockDrugsReporstModel model)
        {
            //var id = _userManager.GetUserId(User);
            var id = _userManager.GetUserId(User);
            var currentDrugs =await _stkDrugsRepository.GetDiscountsForEachStockDrug(id);
            var response = _stkDrugsReportFromExcelService.ProcessFileAndGetReport(id, currentDrugs, model);
            if (!response.Status)
                return BadRequest(Functions.MakeError(response.ErrorMess));
            _transactionService.Begin();
            await _stkDrugsRepository.AddListOfDrugs(response.StkDrugsReport.ToList(), currentDrugs,id);

            try
            {
                _transactionService.CommitChanges().End();
                    return NoContent();
            }
            catch(Exception e)
            {
                _transactionService.RollBackChanges().End();
                return BadRequest(Functions.MakeError(e.Message));
            }         
        }

        #endregion

        #region Get
        [HttpGet("prods",Name ="GetStockDrugsOfReport")]
        public async Task<IActionResult> GetStockDrugsOfReport([FromQuery]LzDrgResourceParameters _params)
        {
            var data = await _stkDrugsRepository.GetAllStockDrugsOfReport(_userManager.GetUserId(User), _params);
            var paginationMetaData = new PaginationMetaDataGenerator<StkShowDrugModel, LzDrgResourceParameters>(
                data, "GetStockDrugsOfReport", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(data);
        }

        [HttpGet("reqs", Name = "GetPharmaciesRequests")]
        public async Task<IActionResult> GetPharmasRequests([FromQuery]PharmaReqsResourceParameters _params)
        {
            var requests = await _stockRepository.GetPharmaRequests(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<ShowPharmaReqToStkModelModel, PharmaReqsResourceParameters>(
                requests, "GetPharmaciesRequests", _params, CreateResourceUriForPhamaReques
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(requests);
        }

        #endregion

        #region Patch
        [HttpPatch("pharmaReqs/{pharmaId}")]
        public async Task<IActionResult> HandlePharmaRequest(string pharmaId, [FromBody] JsonPatchDocument<HandlePharmaRequestModel> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();
            if (!await _stockRepository.HandlePharmacyRequest(pharmaId, request => {

                var model = _mapper.Map<HandlePharmaRequestModel>(request);
                patchDoc.ApplyTo(model);
                //ad validation
                _mapper.Map(model, request);
            }))
                return NotFound();
            return NoContent();
        }

        #endregion

        #region delete
        // DELETE: api/ApiWithActions/5
        [HttpDelete("prods/{id}")]
        public async Task<IActionResult> DeleteDrug(Guid id)
        {
            if (!await _stkDrugsRepository.IsUserHas(id))
                return NotFound();
            var drugToDelete = await _stkDrugsRepository.GetByIdAsync(id);
            _stkDrugsRepository.Delete(drugToDelete);
            if (!await _stkDrugsRepository.SaveAsync())
                return StatusCode(500, Functions.MakeError("حدثت مشكلة اثناء معالجة طلبك"));
            return NoContent();
        }

        

        [HttpDelete("prods")]
        public async Task<IActionResult> DeleteAllDrugs()
        {
            _transactionService.Begin();
            _stkDrugsRepository.DeleteAll();
            _transactionService.CommitChanges();
            /*if (!await _stkDrugsRepository.SaveAsync())
                return StatusCode(500, Functions.MakeError("حدثت مشكلة اثناء معالجة طلبك"));*/
            return NoContent();
        }

        #endregion
    }
}