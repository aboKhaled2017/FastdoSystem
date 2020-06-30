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
    [Route("api/pharmacy/signup")]
    [ApiController]
    [AllowAnonymous]
    public class ParmacySignUpController : SharedAPIController
    {
        private HandlingProofImgsServices _handlingProofImgsServices { get; }
        private IMapper _mapper { get; }
        public PharmacyRepository _pharmacyRepository { get; }

        public ParmacySignUpController(
            UserManager<AppUser> userManager,
            IEmailSender emailSender,
            AccountService accountService,
            TransactionService transactionService,
            HandlingProofImgsServices handlingProofImgsServices,
            IMapper mapper,
            PharmacyRepository pharmacyRepository
            ) 
            : base(userManager, emailSender, accountService, transactionService)
        {
            _handlingProofImgsServices = handlingProofImgsServices;
            _mapper = mapper;
            _pharmacyRepository = pharmacyRepository;
        }


        /// <summary>
        /// register user for new account
        /// </summary>
        /// <example>url=domain/api/account/SignUp,method=post,body={email,fullName,phone,password} </example> 
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SignUp([FromForm]PharmacyClientRegisterModel model)
        {           
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            ISigningResponseModel response = null;
            try
            {
                var pharmacyModel = _mapper.Map<Pharmacy>(model);
                _transactionService.Begin();
                response = await _accountService.SignUpPharmacyAsync(model, (anyErrors, resultError,user)
                    => {
                        if (anyErrors)
                            AddErrors(resultError);
                        else pharmacyModel.Id = user.Id;
                    });
                if (response == null)
                {
                    _transactionService.RollBackChanges();
                    return new UnprocessableEntityObjectResult(ModelState);
                }
                var savingImgsResponse = _handlingProofImgsServices
                    .SavePharmacyProofImgs(model.LicenseImg, model.CommerialRegImg, pharmacyModel.Id);
                if(!savingImgsResponse.Status)
                {
                    ModelState.AddModelError("general", "لا يمكن حفظ الصور,حاول مرة اخرى");
                    _transactionService.RollBackChanges();
                    return new UnprocessableEntityObjectResult(ModelState);
                }
                pharmacyModel.LicenseImgSrc = savingImgsResponse.LicenseImgPath;
                pharmacyModel.CommercialRegImgSrc = savingImgsResponse.CommertialRegImgPath;
                var res=_pharmacyRepository.Add(pharmacyModel);
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