using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Fastdo.backendsys.Models;
using Fastdo.backendsys.Repositories;
using Fastdo.backendsys.Services;
using Fastdo.backendsys.Services.Auth;

namespace Fastdo.backendsys.Controllers.Auth
{
    [Route("api/stk/signup")]
    [ApiController]
    [AllowAnonymous]
    public class StockSignUpController : SharedAPIController
    {
        #region constructor and properties
        private HandlingProofImgsServices _handlingProofImgsServices { get; }
        private IStockRepository _stockRepository { get; }
        public IExecuterDelayer _executerDelayer { get; }

        public StockSignUpController(
            UserManager<AppUser> userManager,
            IEmailSender emailSender, 
            AccountService accountService,
            IMapper mapper,
            HandlingProofImgsServices handlingProofImgsServices,
            IStockRepository stockRepository,
            IExecuterDelayer executerDelayer,
            TransactionService transactionService)
            : base(userManager, emailSender, accountService, mapper, transactionService)
        {
            _handlingProofImgsServices = handlingProofImgsServices;
            _stockRepository = stockRepository;
            _executerDelayer = executerDelayer;
        }

        #endregion

        #region main signup
        [HttpPost]
        public async Task<IActionResult> SignUpForStock([FromForm]StockClientRegisterModel model)
        {           
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            SigningStockClientInResponseModel response = null;
            string ErrorMessage = null;
            try
            {
                var stockModel = _mapper.Map<Stock>(model);
                _transactionService.Begin();
                 response = await _accountService.SignUpStockAsync(
                     model,
                     error=> {
                     _transactionService.RollBackChanges().End();
                     ErrorMessage =error;
                     },
                     (stock,OnFinishAdding)=> {
                         stockModel.Id = stock.Id;
                         var savingImgsResponse = _handlingProofImgsServices
                             .SaveStockProofImgs(model.LicenseImg, model.CommerialRegImg, stockModel.Id);
                         if (!savingImgsResponse.Status)
                         {
                             _transactionService.RollBackChanges().End();
                             ErrorMessage= savingImgsResponse.errorMess;
                         }
                         stockModel.LicenseImgSrc = savingImgsResponse.LicenseImgPath;
                         stockModel.CommercialRegImgSrc = savingImgsResponse.CommertialRegImgPath;
                         _stockRepository.AddAsync(stockModel).Wait();
                         OnFinishAdding.Invoke();
                     },
                     () => {
                         _transactionService.CommitChanges().End();
                     }) as SigningStockClientInResponseModel;
                if (ErrorMessage != null || response== null)
                {
                    return BadRequest(Functions.MakeError(ErrorMessage ?? "لقد فشلت عملية التسجيل,حاول مرة اخرى"));
                }

            }
            catch (Exception ex)
            {
                _transactionService.RollBackChanges().End();
                return BadRequest(new { err = ex.Message });
            }
            return Ok(response);

        }
        #endregion

        #region signup steps
        [HttpPost("step1")]
        public IActionResult SignUpStep1ForStock(Phr_RegisterModel_Identity model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            return Ok();
        }
        [HttpPost("step2")]
        //[Consumes("")]
        public IActionResult SignUpStep2ForStock([FromForm]Phr_RegisterModel_Proof model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            return Ok();
        }
        [HttpPost("step3")]
        public IActionResult SignUpStep3ForStock(Phr_RegisterModel_Contacts model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            return Ok();
        }
        [HttpPost("step4")]
        public IActionResult SignUpStep4ForStock(Phr_RegisterModel_Account model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            return Ok();
        }

        #endregion
    }
}