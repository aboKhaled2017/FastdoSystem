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
using Fastdo.backendsys.Models;
using Fastdo.backendsys.Repositories;
using Fastdo.backendsys.Services.Auth;

namespace Fastdo.backendsys.Controllers
{
    [Route("api/ph/membership")]
    [ApiController]
    [Authorize(Policy ="PharmacyPolicy")]
    public class PhMembershipController : SharedAPIController
    {
        #region constructor and properties
        public IPharmacyRepository _pharmacyRepository { get; }

        public PhMembershipController(
            AccountService accountService,
            IMapper mapper,
            UserManager<AppUser>userManager,
            IPharmacyRepository pharmacyRepository) 
            : base(accountService, mapper,userManager)
        {
            _pharmacyRepository = pharmacyRepository;
        }
        #endregion

        #region patch
        [HttpPatch("name")]
        public async Task<IActionResult> UpdatePhNameForPharmacyOfUser(UpdatePhNameModel model)
        {            
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var pharmacy =await _pharmacyRepository.GetByIdAsync(_userManager.GetUserId(User));
            pharmacy.Name = model.NewName.Trim();
            _pharmacyRepository.UpdateName(pharmacy);
                if(!await _pharmacyRepository.SaveAsync()) return BadRequest(Functions.MakeError("لقد فشلت العملية ,حاول مرة اخرى"));
            var user =await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            var response = await _accountService.GetSigningInResponseModelForCurrentUser(user);
            return Ok(response);
        }

        [HttpPatch("contacts")]
        public async Task<IActionResult> UpdateContactsForPharmacyOfUser(Phr_Contacts_Update model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var pharmacy = await _pharmacyRepository.GetByIdAsync(_userManager.GetUserId(User));
            pharmacy=_mapper.Map(model, pharmacy);
             _pharmacyRepository.UpdateContacts(pharmacy);
            if (!await _pharmacyRepository.SaveAsync()) return BadRequest(Functions.MakeError("لقد فشلت العملية ,حاول مرة اخرى"));
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            var response = await _accountService.GetSigningInResponseModelForCurrentUser(user);
            return Ok(response);
        }

        #endregion
    }
}