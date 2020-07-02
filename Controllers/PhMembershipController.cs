using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System_Back_End.Models;
using System_Back_End.Repositories;
using System_Back_End.Services.Auth;

namespace System_Back_End.Controllers
{
    [Route("api/ph/membership")]
    [ApiController]
    [Authorize(Policy ="PharmacyPolicy")]
    public class PhMembershipController : SharedAPIController
    {
        public PharmacyRepository _pharmacyRepository { get; }

        public PhMembershipController(
            AccountService accountService,
            IMapper mapper,
            UserManager<AppUser>userManager,
            PharmacyRepository pharmacyRepository) 
            : base(accountService, mapper,userManager)
        {
            _pharmacyRepository = pharmacyRepository;
        }
        [HttpPatch("name")]
        public async Task<IActionResult> UpdatePhName(UpdatePhNameModel model)
        {            
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var pharmacy =await _pharmacyRepository.GetByIdAsync(_userManager.GetUserId(User));
            pharmacy.Name = model.NewName.Trim();
            var res = await _pharmacyRepository.UpdateFields<Pharmacy>(pharmacy, prop => prop.Name);
                if(!res) return BadRequest(Functions.MakeError("لقد فشلت العملية ,حاول مرة اخرى"));
            var user =await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            var response = await _accountService.GetSigningInResponseModelForCurrentUser(user);
            return Ok(response);
        }

        [HttpPatch("contacts")]
        public async Task<IActionResult> UpdateContacts(Phr_Contacts_Update model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var pharmacy = await _pharmacyRepository.GetByIdAsync(_userManager.GetUserId(User));
            pharmacy=_mapper.Map(model, pharmacy);
            var res = await _pharmacyRepository.UpdateFields<Pharmacy>(pharmacy,
                prop => prop.PersPhone,
                prop=>prop.LandlinePhone,
                prop=>prop.Address
                );
            if (!res) return BadRequest(Functions.MakeError("لقد فشلت العملية ,حاول مرة اخرى"));
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            var response = await _accountService.GetSigningInResponseModelForCurrentUser(user);
            return Ok(response);
        }
    }
}