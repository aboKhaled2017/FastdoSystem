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
    [Route("api/stk/membership")]
    [ApiController]
    [Authorize(Policy ="StockPolicy")]
    public class StkMembershipController : SharedAPIController
    {
        #region constructor and properties
        public IStockRepository _stockRepository { get; }

        public StkMembershipController(
            AccountService accountService,
            IMapper mapper,
            UserManager<AppUser>userManager,
            IStockRepository stockRepository) 
            : base(accountService, mapper,userManager)
        {
            _stockRepository = stockRepository;
        }

        #endregion

        #region patch
        [HttpPatch("name")]
        public async Task<IActionResult> UpdatePhNameForStockOfUser(UpdateStkNameModel model)
        {            
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var stock = await _stockRepository.GetByIdAsync(_userManager.GetUserId(User));
            stock.Name = model.NewName.Trim();
            _stockRepository.UpdateName(stock);
            if(!await _stockRepository.SaveAsync()) return BadRequest(Functions.MakeError("لقد فشلت العملية ,حاول مرة اخرى"));
            var user =await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            var response = await _accountService.GetSigningInResponseModelForCurrentUser(user);
            return Ok(response);
        }

        [HttpPatch("contacts")]
        public async Task<IActionResult> UpdateContactsForStockOfUser(Stk_Contacts_Update model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var stock = await _stockRepository.GetByIdAsync(_userManager.GetUserId(User));
            stock = _mapper.Map(model, stock);
            _stockRepository.UpdateContacts(stock);
            if (!await _stockRepository.SaveAsync()) return BadRequest(Functions.MakeError("لقد فشلت العملية ,حاول مرة اخرى"));
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            var response = await _accountService.GetSigningInResponseModelForCurrentUser(user);
            return Ok(response);
        }
        #endregion
    }
}