using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System_Back_End.Models;
using System_Back_End.Services;
using System_Back_End.Services.Auth;

namespace System_Back_End.Controllers.Auth
{
    [Route("api/auth/[action]")]
    [ApiController]
    public class AuthController : SharedAPIController
    {
        public AuthController(UserManager<AppUser> userManager, IEmailSender emailSender, AccountService accountService, TransactionService transactionService) : base(userManager, emailSender, accountService, transactionService)
        {
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            _accountService.SetCurrentContext(
                actionContext.HttpContext,
                new UrlHelper(actionContext)
                );
        }
        #region Regular signin/signup

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(LoginModel model)
        {
            if (model == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (await _userManager.UserIdentityExists(user, model.Password,model.UserType))
                {
                    if (!user.EmailConfirmed)
                    {
                        return Unauthorized(new { err = "البريد الالكترونى غير مؤكد" });
                    }
                    var response = model.UserType == UserType.pharmacier
                        ?await _accountService.GetSigningInResponseModelForPharmacy(user)
                        :await _accountService.GetSigningInResponseModelForStock(user);
                    return Ok(response);
                }
                return NotFound(new {err= "البريد الالكترونى او كلمة السر غير صحيحة" });
            }
            catch (Exception ex)
            {
                return BadRequest(new {err=ex.Message});
            }

        }
        #endregion

        #region for email settings
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SendMeEmailConfirmCode(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }

            var code =await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var callbackUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/auth/confirmemail?userId={user.Id}&code={code}";
            var callbackUrl = Url.EmailConfirmationLink(user.Id, code, HttpContext.Request.Scheme);
            await _emailSender.SendEmailAsync(user.Email, "كود تأكيد البريد الالكترونى", $"اضغط <a href='{callbackUrl}'>هنا</a> لتأكيد البريد الالكترونى");
            return Ok();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return Ok("تم تأكيد بريدك الالكترونى, ارجع الى الموقع مرة اخرى وقم بتحديث الصفحة");
            }
            else return BadRequest(new { err = "error occured on confirming your email" });
        }
        #endregion
        #region for password setting
        /// <summary>
        /// try to retrieve user's forgotton password
        /// </summary>
        /// <example>url=domain/api/account/ForgotPassword method=post,body={email}</example>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
             
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound();
            if (!(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return new NotConfirmedEmailResult();
            }
            user.confirmCode=Functions.GetRandomDigits(15);
            var res = await _userManager.UpdateAsync(user);
            //var code =await _userManager.GeneratePasswordResetTokenAsync(user);
            //var callbackUrl = Url.ResetPasswordCallbackLink(user.Id.ToString(), code,model.NewPassword, Request.Scheme);
            await _emailSender.SendEmailAsync(model.Email, "اعادة ضبط كلمة المرور",$"الكود الخاص بتغيير كلمة المرور هو: {user.confirmCode}");
            return Ok();
            
        }
        /// <summary>
        /// reset user password
        /// </summary>
        /// <example>url=domain/api/account/ResetPassword method=post,body={email,code,password,confirmPassword}</example>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound();
            if (!user.confirmCode.Equals(model.Code))
                return NotFound();
            //var result = await _userManager.ResetPasswordAsync(user, model.Code, model.NewPassword);
            var code =await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user,code, model.NewPassword);
            if (result.Succeeded)
                return Ok();
            else
            {
                ModelState.AddModelError("code", "الكود الذى ادخلتة غير صحيح");
                return new UnprocessableEntityObjectResult(ModelState);
            }       
        }
        #endregion
    }
}