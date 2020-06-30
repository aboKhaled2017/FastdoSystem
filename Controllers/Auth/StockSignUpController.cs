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
    [Route("api/stock/signup")]
    [ApiController]
    [AllowAnonymous]
    public class StockSignUpController : SharedAPIController
    {
        private HandlingProofImgsServices _handlingProofImgsServices { get; }
        private IMapper _mapper { get; }
        public StockRepository _stockRepository { get; }

        public StockSignUpController(
            UserManager<AppUser> userManager,
            IEmailSender emailSender,
            AccountService accountService,
            TransactionService transactionService,
            HandlingProofImgsServices handlingProofImgsServices,
            IMapper mapper,
            StockRepository stockRepository
            ) 
            : base(userManager, emailSender, accountService, transactionService)
        {
            _handlingProofImgsServices = handlingProofImgsServices;
            _mapper = mapper;
            _stockRepository = stockRepository;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromForm]StockClientRegisterModel model)
        {           
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            ISigningResponseModel response = null;
            try
            {
                var stockModel = _mapper.Map<Stock>(model);
                _transactionService.Begin();
                 response = await _accountService.SignUpStockAsync(model, (anyErrors, resultError,user)
                    => {
                        if (anyErrors)
                            AddErrors(resultError);
                        else stockModel.Id = user.Id;
                    });
                if (response == null)
                {
                    _transactionService.RollBackChanges();
                    return new UnprocessableEntityObjectResult(ModelState);
                }
                var savingImgsResponse = _handlingProofImgsServices
                    .SavePharmacyProofImgs(model.LicenseImg, model.CommerialRegImg, stockModel.Id);
                if(!savingImgsResponse.Status)
                {
                    ModelState.AddModelError("general", "لا يمكن حفظ الصور,حاول مرة اخرى");
                    _transactionService.RollBackChanges();
                    return new UnprocessableEntityObjectResult(ModelState);
                }
                stockModel.LicenseImgSrc = savingImgsResponse.LicenseImgPath;
                stockModel.CommercialRegImgSrc = savingImgsResponse.CommertialRegImgPath;
                var res=_stockRepository.Add(stockModel);
                if(!res)
                {
                    _transactionService.RollBackChanges();
                    return BadRequest();
                }
                _transactionService.CommitChanges();               

            }
            catch (Exception ex)
            {
                _transactionService.RollBackChanges();
                return BadRequest(new { err = ex.Message });
            }
            finally
            {
                _transactionService.End();
            }
            return Ok(response);

        }

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

    }
}