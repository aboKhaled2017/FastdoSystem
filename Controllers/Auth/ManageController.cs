using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Models;
using System.Net.Http;
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
using System_Back_End.Utilities;

namespace System_Back_End.Controllers.Auth
{
    [Route("api/manage")]
    [ApiController]
    [Authorize]
    public class ManageController : SharedAPIController
    {
        public PharmacyRepository _pharmacyRepository { get; }
        public StockRepository _stockRepository { get; }

        public ManageController(
            UserManager<AppUser> userManager, 
            IEmailSender emailSender, AccountService accountService, 
            IMapper mapper, TransactionService transactionService,
            PharmacyRepository pharmacyRepository,StockRepository stockRepository)
            : base(userManager, emailSender, accountService, mapper, transactionService)
        {
            _pharmacyRepository = pharmacyRepository;
            _stockRepository = stockRepository;
        }

        [HttpPost("password")]       
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            var user =await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            var res =await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if(!res.Succeeded)
                return NotFound(Functions.MakeError("OldPassword", "كلمة السر القديمة غير صحيحة"));
            return Ok();
        }
        [HttpPost("ph/phone")]
        public async Task<IActionResult> ChangePhoneForPharmacy(ChangePhoneModel model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            _transactionService.Begin();
            var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.NewPhone);
            var res = await _userManager.ChangePhoneNumberAsync(user, model.NewPhone, token);
            if (!res.Succeeded)
            {
                _transactionService.RollBackChanges().End();
                return BadRequest(Functions.MakeError("لايمكن تغير رقم الهاتف الان ,حاول مرة اخرى"));
            } 
            var pharmacy =await _pharmacyRepository.GetByIdAsync(user.Id);
            if (pharmacy == null)
            {
                _transactionService.RollBackChanges().End();
                return BadRequest(Functions.MakeError("لايمكن تغير رقم الهاتف الان ,حاول مرة اخرى"));
            }
            pharmacy.PersPhone = model.NewPhone;
            var result = await _pharmacyRepository.UpdateFields<Pharmacy>(pharmacy, props => props.PersPhone);
            if (!result)
            {
                _transactionService.RollBackChanges().End();
                return BadRequest(Functions.MakeError("لايمكن تغير رقم الهاتف الان ,حاول مرة اخرى"));
            }
            var response =await _accountService.GetSigningInResponseModelForCurrentUser(user);
            _transactionService.CommitChanges().End();
            return Ok(response);
        }
        [HttpPost("stk/phone")]
        public async Task<IActionResult> ChangePhoneForStock(ChangePhoneModel model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            _transactionService.Begin();
            var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.NewPhone);
            var res = await _userManager.ChangePhoneNumberAsync(user, model.NewPhone, token);
            if (!res.Succeeded)
            {
                _transactionService.RollBackChanges().End();
                return BadRequest(Functions.MakeError("لايمكن تغير رقم الهاتف الان ,حاول مرة اخرى"));
            }
            var stock = await _stockRepository.GetByIdAsync(user.Id);
            if (stock == null)
            {
                _transactionService.RollBackChanges().End();
                return BadRequest(Functions.MakeError("لايمكن تغير رقم الهاتف الان ,حاول مرة اخرى"));
            }
            stock.PersPhone = model.NewPhone;
            var result = await _stockRepository.UpdateFields<Stock>(stock, props => props.PersPhone);
            if (!result)
            {
                _transactionService.RollBackChanges().End();
                return BadRequest(Functions.MakeError("لايمكن تغير رقم الهاتف الان ,حاول مرة اخرى"));
            }
            var response = await _accountService.GetSigningInResponseModelForCurrentUser(user);
            _transactionService.CommitChanges().End();
            return Ok(response);
        }

        [HttpGet("email")]
        public async Task<IActionResult> ChangeEmail(ChangeEmailModel model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var user =await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            var code = Functions.GenerateConfirmationTokenCode();
            user.confirmCode = code;
            user.willBeNewEmail = model.NewEmail;
            var res=await _userManager.UpdateAsync(user);
            if (!res.Succeeded)
                return BadRequest(Functions.MakeError("لايمكن تغير البريد الالكترونى الان ,حاول مرة اخرى"));
            await _emailSender.SendEmailAsync(model.NewEmail, " تغيير البريد الالكترونى الخاص بك", $"الكود الخاص بك هو {code}");
            return Ok();
        }
        [HttpGet("sendCodeToMailAgain")]
        public async Task<IActionResult> SendCodeToChangedEmailAgain(ChangeEmailModel model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (user==null||user.willBeNewEmail!=model.NewEmail)
                return NotFound();
            if (user.confirmCode == null)
                return BadRequest();
            await _emailSender.SendEmailAsync(model.NewEmail, " تغيير البريد الالكترونى الخاص بك", $"الكود الخاص بك هو {user.confirmCode}");
            return Ok();
        }
        [HttpPost("email")]
        public async Task<IActionResult> ConfirmChangeEmail(ConfirmChangeEmailModel model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (user.confirmCode == null)
                return NotFound(Functions.MakeError("Code", "انتهت صلاحية هذا الكود"));
            if (user.confirmCode != model.Code)
                return NotFound(Functions.MakeError("Code", "هذا الكود غير صحيح"));
            if (user.willBeNewEmail != model.NewEmail)
                return NotFound();
            user.UserName = model.NewEmail;
            user.willBeNewEmail = null;
            user.EmailConfirmed = true;
            _transactionService.Begin();
            try
            {
                var token = await _userManager.GenerateChangeEmailTokenAsync(user, model.NewEmail);
                var result1 = await _userManager.ChangeEmailAsync(user, model.NewEmail, token);
                var result2 =await _userManager.UpdateAsync(user);
                if (!result1.Succeeded||!result2.Succeeded)
                    return NotFound(Functions.MakeError("لقد فشلت العملية ,حاول مرة اخرى" ));
                _transactionService.CommitChanges().End();
            }
            catch
            {
                _transactionService.RollBackChanges().End();
                return BadRequest(Functions.MakeError("لقد فشلت العملية ,حاول مرة اخرى"));
            }
            
            return Ok(await _accountService.GetSigningInResponseModelForCurrentUser(user));
        }
    }
}