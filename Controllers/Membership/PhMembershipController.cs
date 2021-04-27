using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Fastdo.Core.ViewModels;
using Fastdo.API.Repositories;
using Fastdo.API.Services.Auth;
using Microsoft.AspNetCore.Identity.UI.Services;
using Fastdo.Core.Services;
using Fastdo.Core.Utilities;
using Fastdo.Core.Services.Auth;

namespace Fastdo.API.Controllers
{
    [Route("api/ph/membership")]
    [ApiController]
    [Authorize(Policy ="PharmacyPolicy")]
    public class PhMembershipController : SharedAPIController
    {
        public PhMembershipController(UserManager<AppUser> userManager, IEmailSender emailSender, IAccountService accountService, IMapper mapper, ITransactionService transactionService, Core.IUnitOfWork unitOfWork) : base(userManager, emailSender, accountService, mapper, transactionService, unitOfWork)
        {
        }

        #region patch
        [HttpPatch("name")]
        public async Task<IActionResult> UpdatePhNameForPharmacyOfUser(UpdatePhNameModel model)
        {            
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var pharmacy =await _unitOfWork.PharmacyRepository.GetByIdAsync(_userManager.GetUserId(User));
            pharmacy.Name = model.NewName.Trim();
             _unitOfWork.PharmacyRepository.UpdateName(pharmacy);
                if(!await  _unitOfWork.PharmacyRepository.SaveAsync()) return BadRequest(BasicUtility.MakeError("لقد فشلت العملية ,حاول مرة اخرى"));
            var user =await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            var response = await _accountService.GetSigningInResponseModelForCurrentUser(user);
            return Ok(response);
        }

        [HttpPatch("contacts")]
        public async Task<IActionResult> UpdateContactsForPharmacyOfUser(Phr_Contacts_Update model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var pharmacy = await  _unitOfWork.PharmacyRepository.GetByIdAsync(_userManager.GetUserId(User));
            pharmacy=_mapper.Map(model, pharmacy);
              _unitOfWork.PharmacyRepository.UpdateContacts(pharmacy);
            if (!await  _unitOfWork.PharmacyRepository.SaveAsync()) return BadRequest(BasicUtility.MakeError("لقد فشلت العملية ,حاول مرة اخرى"));
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            var response = await _accountService.GetSigningInResponseModelForCurrentUser(user);
            return Ok(response);
        }

        #endregion
    }
}