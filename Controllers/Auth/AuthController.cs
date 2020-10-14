using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Fastdo.backendsys.Models;
using Fastdo.backendsys.Services;
using Fastdo.backendsys.Services.Auth;
using Fastdo.backendsys.Utilities;

namespace Fastdo.backendsys.Controllers.Auth
{
    [Route("api/auth/[action]")]
    [ApiController]
    public class AuthController : SharedAPIController
    {
        #region constructor and properties
        public AuthController(UserManager<AppUser> userManager, IEmailSender emailSender, AccountService accountService, IMapper mapper, TransactionService transactionService) : base(userManager, emailSender, accountService, mapper, transactionService)
        {
        }
        #endregion

        #region ovveride methods
        [ApiExplorerSettings(IgnoreApi = true)]
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            _accountService.SetCurrentContext(
                actionContext.HttpContext,
                new UrlHelper(actionContext)
                );
        }
        #endregion


        #region signIn

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
                    //user can continue with confirming
                    /*if (!user.EmailConfirmed)
                    {
                        return Unauthorized(Functions.MakeError("البريد الالكترونى غير مفعل ,من فضلك قم بتأكيد بريدك الالكترونى"));
                    }*/
                    var response = model.UserType == UserType.pharmacier
                        ?await _accountService.GetSigningInResponseModelForPharmacy(user)
                        :await _accountService.GetSigningInResponseModelForStock(user);
                    return Ok(response);
                }
                return NotFound(Functions.MakeError("البريد الالكترونى او كلمة السر غير صحيحة" ));
            }
            catch (Exception ex)
            {
                return BadRequest(Functions.MakeError(ex.Message));
            }

        }
        #endregion



        #region email [confirm/sendConfirmAgain]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SendMeEmailConfirmCodeAgain([EmailAddress(ErrorMessage ="email is not valid")][Required(ErrorMessage ="email is required")]string email)
        {

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound();
            if (user.EmailConfirmed)
                return BadRequest(Functions.MakeError("G", "هذا الايميل بالفعل مفعل!"));

            //var code =await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var callbackUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/auth/confirmemail?userId={user.Id}&code={code}";
            //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, HttpContext.Request.Scheme);
            /*user.confirmCode = Functions.GenerateConfirmationTokenCode();
            var res = await _userManager.UpdateAsync(user);
            if (!res.Succeeded)
                return BadRequest(Functions.MakeError("Email","حدثت مشكلة فى السيرفر,حاول مرة اخرى"));*/
            await _emailSender.SendEmailAsync(user.Email, "كود تأكيد البريد الالكترونى", $"كود التأكيد الخاص بك هو: {user.confirmCode}");
            return Ok();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailModel model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound();
            if (user.EmailConfirmed)
                return BadRequest(Functions.MakeError("G", "هذا البريد الالكترونى بفعل بالفعل"));
            if (user.confirmCode==null || !user.confirmCode.Equals(model.Code))
                return NotFound(Functions.MakeError("Code", "الكود الذى ادخلتة غير صحيح"));
            user.confirmCode = null;
            var res =await _userManager.UpdateAsync(user);
            if (!res.Succeeded)
                return BadRequest(Functions.MakeError("G", "فشلت العملية,حاول مرة اخرى"));
            var token =await _userManager.GenerateEmailConfirmationTokenAsync(user);
            res = await _userManager.ConfirmEmailAsync(user, token);
            if(!res.Succeeded)
                return BadRequest(Functions.MakeError("G", "فشلت العملية,حاول مرة اخرى"));
            return Ok();
        }
        #endregion


        #region for password [forgot/reset]
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
            user.confirmCode=Functions.GenerateConfirmationTokenCode();
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
            if (user.confirmCode == null)
                return BadRequest();
            if (!user.confirmCode.Equals(model.Code))
                return NotFound(Functions.MakeError("Code","الكود الذى ادخلته غير صحيح"));
            user.confirmCode = null;
            var res = await _userManager.UpdateAsync(user);
            if (!res.Succeeded)
                return BadRequest(Functions.MakeError("فشلت العملية ,حاول مرة اخرى" ));
            //var result = await _userManager.ResetPasswordAsync(user, model.Code, model.NewPassword);
            var token =await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user,token, model.NewPassword);
            if (!result.Succeeded)
                return BadRequest(Functions.MakeError("فشلت العملية ,حاول مرة اخرى"));
            return Ok();      
        }
        #endregion
    }
}