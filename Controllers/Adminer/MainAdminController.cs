using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fastdo.backendsys.Services;
using Fastdo.backendsys.Services.Auth;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fastdo.backendsys.Controllers.Adminer
{
    [Authorize(Policy = "AdministratorPolicy")]
    public class MainAdminController : SharedAPIController
    {
        public MainAdminController(AccountService accountService, IMapper mapper, UserManager<AppUser> userManager) : base(accountService, mapper, userManager)
        {
        }

        public MainAdminController(UserManager<AppUser> userManager, IEmailSender emailSender, AccountService accountService, IMapper mapper, TransactionService transactionService) : base(userManager, emailSender, accountService, mapper, transactionService)
        {
        }
    }
}