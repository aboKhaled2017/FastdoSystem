using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System_Back_End.Services;
using System_Back_End.Services.Auth;

namespace System_Back_End.Controllers
{
        public class SharedAPIController : Controller
        {
        protected readonly UserManager<AppUser> _userManager;
        protected readonly IEmailSender _emailSender;
        protected readonly AccountService _accountService;
        protected readonly TransactionService _transactionService;
        public SharedAPIController(
            UserManager<AppUser> userManager,
            IEmailSender emailSender,
            AccountService accountService,
            TransactionService transactionService)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _accountService = accountService;
            _transactionService = transactionService;
        }
        protected void AddErrors(IdentityResult result)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }
}