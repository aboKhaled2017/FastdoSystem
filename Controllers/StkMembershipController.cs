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
    [Route("api/stk/membership")]
    [ApiController]
    [Authorize(Policy ="StockPolicy")]
    public class StkMembershipController : SharedAPIController
    {
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
        [HttpPatch("name")]
        public async Task<IActionResult> UpdatePhName(UpdateStkNameModel model)
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
        public async Task<IActionResult> UpdateContacts(Stk_Contacts_Update model)
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
    }
}