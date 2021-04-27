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
    [Route("api/stk/membership")]
    [ApiController]
    [Authorize(Policy ="StockPolicy")]
    public class StkMembershipController : SharedAPIController
    {
        public StkMembershipController(UserManager<AppUser> userManager, IEmailSender emailSender, IAccountService accountService, IMapper mapper, ITransactionService transactionService, Core.IUnitOfWork unitOfWork) : base(userManager, emailSender, accountService, mapper, transactionService, unitOfWork)
        {
        }


        #region patch
        [HttpPatch("name")]
        public async Task<IActionResult> UpdatePhNameForStockOfUser(UpdateStkNameModel model)
        {            
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var stock = await _unitOfWork.StockRepository.GetByIdAsync(_userManager.GetUserId(User));
            stock.Name = model.NewName.Trim();
            _unitOfWork.StockRepository.UpdateName(stock);
            if(!await _unitOfWork.StockRepository.SaveAsync()) return BadRequest(BasicUtility.MakeError("لقد فشلت العملية ,حاول مرة اخرى"));
            var user =await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            var response = await _accountService.GetSigningInResponseModelForCurrentUser(user);
            return Ok(response);
        }

        [HttpPatch("contacts")]
        public async Task<IActionResult> UpdateContactsForStockOfUser(Stk_Contacts_Update model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var stock = await _unitOfWork.StockRepository.GetByIdAsync(_userManager.GetUserId(User));
            stock = _mapper.Map(model, stock);
            _unitOfWork.StockRepository.UpdateContacts(stock);
            if (!await _unitOfWork.StockRepository.SaveAsync()) return BadRequest(BasicUtility.MakeError("لقد فشلت العملية ,حاول مرة اخرى"));
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            var response = await _accountService.GetSigningInResponseModelForCurrentUser(user);
            return Ok(response);
        }
        #endregion
    }
}