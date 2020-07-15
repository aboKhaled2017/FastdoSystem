using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System_Back_End.Models;
using System_Back_End.Repositories;
using System_Back_End.Services;
using System_Back_End.Services.Auth;

namespace System_Back_End.Controllers.Auth
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
        public async Task<IActionResult> SignUp([FromForm]StockClientRegisterModel model)
        {           
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            SigningStockClientInResponseModel response = null;
            try
            {
                var stockModel = _mapper.Map<Stock>(model);
                _transactionService.Begin();
                 response = await _accountService.SignUpStockAsync(model,_executerDelayer) as SigningStockClientInResponseModel;
                if (response == null)
                {
                    _transactionService.RollBackChanges().End();
                    return BadRequest(Functions.MakeError("لقد فشلت عملية التسجيل,حاول مرة اخرى"));
                }
                stockModel.Id = response.user.Id;               
                var savingImgsResponse = _handlingProofImgsServices
                    .SaveStockProofImgs(model.LicenseImg, model.CommerialRegImg, stockModel.Id);
                if(!savingImgsResponse.Status)
                {
                    _transactionService.RollBackChanges().End();
                    return BadRequest(Functions.MakeError(savingImgsResponse.errorMess));
                }
                stockModel.LicenseImgSrc = savingImgsResponse.LicenseImgPath;
                stockModel.CommercialRegImgSrc = savingImgsResponse.CommertialRegImgPath;
                var res=await _stockRepository.AddAsync(stockModel);
                if(!res)
                {
                    _transactionService.RollBackChanges().End();
                    return BadRequest(Functions.MakeError("لقد فشلت عملية التسجيل,حاول مرة اخرى"));
                }
                _executerDelayer.Execute();
                _transactionService.CommitChanges().End();               

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
        public IActionResult SignUp_Step1(Phr_RegisterModel_Identity model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            return Ok();
        }
        [HttpPost("step2")]
        //[Consumes("")]
        public IActionResult SignUp_Step2([FromForm]Phr_RegisterModel_Proof model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            return Ok();
        }
        [HttpPost("step3")]
        public IActionResult SignUp_Step3(Phr_RegisterModel_Contacts model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            return Ok();
        }
        [HttpPost("step4")]
        public IActionResult SignUp_Step4(Phr_RegisterModel_Account model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            return Ok();
        }

        #endregion
    }
}