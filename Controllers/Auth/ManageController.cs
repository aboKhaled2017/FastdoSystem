using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System_Back_End.Models;
using System_Back_End.Services;
using System_Back_End.Services.Auth;

namespace System_Back_End.Controllers.Auth
{
    [Route("api/manage")]
    [ApiController]
    [Authorize]
    public class ManageController : SharedAPIController
    {
        public ManageController(UserManager<AppUser> userManager, IEmailSender emailSender, AccountService accountService, TransactionService transactionService) : base(userManager, emailSender, accountService, transactionService)
        {
        }

        [HttpPost("password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            var user =await _userManager.FindByIdAsync(Properties.UserIdentifier.UserId);
            var res =await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if(!res.Succeeded)
                return NotFound(new { err = "كلمة السر القديمة غير صحيحة" });
            return Ok();
        }
        [HttpGet("email")]
        public async Task<IActionResult> ChangeEmail(string email)
        {
            if (email == null)
                return BadRequest();
            var user =await _userManager.FindByIdAsync(Properties.UserIdentifier.UserId);
            var code = Functions.GetRandomDigits(15);
            user.confirmCode = code;
            var res=await _userManager.UpdateAsync(user);
            if (!res.Succeeded)
                return BadRequest("لايمكن تغير البريد الالكترونى الان ,حاول مرة اخرى");
            await _emailSender.SendEmailAsync(email, " تغيير البريد الالكترونى الخاص بك", $"الكود الخاص بك هو {code}");
            return Ok();
        }
        [HttpPost("email")]
        public async Task<IActionResult> ConfirmChangeEmail(string newEmail,string code)
        {
            if (newEmail == null||code==null)
                return BadRequest();
            var user = await _userManager.FindByIdAsync(Properties.UserIdentifier.UserId);
            if (user.confirmCode != code)
                return NotFound(new { err = "الكود غير صحيح" });
            user.Email = newEmail;
            user.UserName = newEmail;
            user.EmailConfirmed = true;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return NotFound(new { err = "الكود غير صحيح" });
            return Ok(await _accountService.GetSigningInResponseModelForCurrentUser(user));
        }
    }
}