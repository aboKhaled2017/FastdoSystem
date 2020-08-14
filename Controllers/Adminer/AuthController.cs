using System;
using System.Models;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System_Back_End.Models;
using System_Back_End.Repositories;
using System_Back_End.Services;
using System_Back_End.Services.Auth;

namespace System_Back_End.Controllers.Adminer
{
    [Route("api/admin/auth")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    public class AuthController : SharedAPIController
    {
        #region constructor and properties
        public AuthController(UserManager<AppUser> userManager, IEmailSender emailSender, AccountService accountService, IMapper mapper, TransactionService transactionService) : base(userManager, emailSender, accountService, mapper, transactionService)
        {
        }
        #endregion

        #region ovveride methods
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            _accountService.SetCurrentContext(
                actionContext.HttpContext,
                new UrlHelper(actionContext)
                );
        }
        #endregion


        #region signIn

        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(AdminLoginModel model)
        {
            if (model == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            try
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (await _userManager.UserIdentityExists(user, model.Password, model.AdminType))
                {
                    var response = await _accountService.GetSigningInResponseModelForAdministrator(user,model.AdminType);
                    return Ok(response);
                }
                return NotFound(Functions.MakeError("اسم المستخدم او كلمة السر غير صحيحة"));
            }
            catch (Exception ex)
            {
                return BadRequest(Functions.MakeError(ex.Message));
            }

        }
        #endregion

        
    }
}