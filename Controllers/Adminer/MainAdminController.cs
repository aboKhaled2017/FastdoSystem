using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fastdo.API.Services;
using Fastdo.API.Services.Auth;
using Fastdo.Core;
using Fastdo.Core.Models;
using Fastdo.Core.Services;
using Fastdo.Core.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fastdo.API.Controllers.Adminer
{
    [Authorize(Policy = "AdministratorPolicy")]
    public class MainAdminController : SharedAPIController
    {
        public MainAdminController(IAccountService accountService, IMapper mapper, UserManager<AppUser> userManager) : base(accountService, mapper, userManager)
        {
        }

        public MainAdminController(UserManager<AppUser> userManager, IEmailSender emailSender, IAccountService accountService, IMapper mapper, ITransactionService transactionService, IUnitOfWork unitOfWork) : base(userManager, emailSender, accountService, mapper, transactionService, unitOfWork)
        {
        }
    }
}