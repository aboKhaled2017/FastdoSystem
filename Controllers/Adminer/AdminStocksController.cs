using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fastdo.backendsys.Models;
using Fastdo.backendsys.Repositories;
using Fastdo.backendsys.Services;
using Fastdo.backendsys.Services.Auth;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Fastdo.backendsys.Controllers.Adminer
{
    [Route("api/admins/stocks", Name = "AdminStocks")]
    [ApiController]
    [Authorize(Policy = "ControlOnStocksPagePolicy")]
    public class AdminStocksController : MainAdminController
    {
        #region constructor and properties
        private readonly IStockRepository _stockRepository;
        public AdminStocksController(UserManager<AppUser> userManager, IEmailSender emailSender,
            AccountService accountService, IMapper mapper, TransactionService transactionService,
            IStockRepository stockRepository) : base(userManager, emailSender, accountService, mapper, transactionService)
        {
            _stockRepository = stockRepository;
        }
        #endregion

        #region override methods from parent class
        [ApiExplorerSettings(IgnoreApi = true)]
        public override string Create_BMs_ResourceUri(ResourceParameters _params, ResourceUriType resourceUriType, string routeName)
        {
            var _cardParams = _params as StockResourceParameters;
            switch (resourceUriType)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(routeName,
                    new StockResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber - 1,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S,
                        Status = _cardParams.Status,
                        OrderBy = _cardParams.OrderBy
                    });
                case ResourceUriType.NextPage:
                    return Url.Link(routeName,
                    new StockResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber + 1,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S,
                        Status = _cardParams.Status,
                        OrderBy = _cardParams.OrderBy
                    });
                default:
                    return Url.Link(routeName,
                    new StockResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S,
                        Status=_cardParams.Status,
                        OrderBy = _cardParams.OrderBy
                    });
            }
        }


        #endregion

        #region get
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStockByIdForAdmin([FromRoute]string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();
            var stk =await _stockRepository.Get_StockModel_ADM(id);
            if (stk == null)
                return NotFound();
            return Ok(stk);
        }
        [HttpGet(Name ="Get_PageOfStocks_ADM")]
        public async Task<IActionResult> GetPageOfStocksForAdmin([FromQuery]StockResourceParameters _params)
        {
            var stocks = await _stockRepository.Get_PageOf_StockModels_ADM(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<Get_PageOf_Stocks_ADMModel, StockResourceParameters>(
                stocks, "Get_PageOfStocks_ADM", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(stocks);
        }
        #endregion

        #region delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStockForAdmin([FromRoute]string id)
        {
            var stk = await _stockRepository.GetByIdAsync(id);
            if (stk == null)
                return NotFound();
             _stockRepository.Remove(stk);
            if (!await _stockRepository.SaveAsync())
                return StatusCode(500, Functions.MakeError("حدثت مشكلة اثناء معالجة طلبك ,من فضلك حاول مرة اخرى"));
            return NoContent();
        }
        #endregion

        #region Patch
        [HttpPatch("{id}")]
        public async Task<IActionResult> HandleStockRequestForAdmin([FromRoute]string id, [FromBody] JsonPatchDocument<Stock_Update_ADM_Model> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();
            var stk = await _stockRepository.GetByIdAsync(id);
            if (stk == null)
                return NotFound();
            var requestToPatch = _mapper.Map<Stock_Update_ADM_Model>(stk);
            patchDoc.ApplyTo(requestToPatch);
            //ad validation
            _mapper.Map(requestToPatch, stk);
            var isSuccessfulluUpdated = await _stockRepository.Patch_Apdate_ByAdmin(stk);
            if (!isSuccessfulluUpdated)
                return StatusCode(500, Functions.MakeError("لقد حدثت مشكلة اثناء معالجة طلبك , من فضلك حاول مرة اخرى"));
            return NoContent();
        }
        #endregion
    }
}