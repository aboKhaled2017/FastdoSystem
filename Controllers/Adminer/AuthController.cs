using System;
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
using Fastdo.backendsys.Repositories;
using Fastdo.backendsys.Services;
using Fastdo.backendsys.Services.Auth;
 

namespace Fastdo.backendsys.Controllers.Adminer
{
    [Route("api/admin/auth", Name = "AdminAuth")]
    [ApiController]
    public class AdminAuthController : MainAdminController
    {
        #region constructor and properties
        public AdminAuthController(UserManager<AppUser> userManager, IEmailSender emailSender, AccountService accountService, IMapper mapper, TransactionService transactionService) : base(userManager, emailSender, accountService, mapper, transactionService)
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

        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> SignInAdminAsync(AdminLoginModel model)
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