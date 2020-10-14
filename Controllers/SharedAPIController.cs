using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Fastdo.backendsys.Services;
using Fastdo.backendsys.Services.Auth;

namespace Fastdo.backendsys.Controllers
{
    public class SharedAPIController : Controller
    {
        #region constructor and properties
        protected readonly UserManager<AppUser> _userManager;
        protected readonly IEmailSender _emailSender;
        protected readonly AccountService _accountService;
        protected readonly TransactionService _transactionService;
        protected readonly IMapper _mapper;
        public SharedAPIController(
            UserManager<AppUser> userManager,
            IEmailSender emailSender,
            AccountService accountService,
            IMapper mapper,
            TransactionService transactionService)
            :this(accountService,mapper,userManager)
        {           
            _emailSender = emailSender;     
            _transactionService = transactionService;    
        }

        public SharedAPIController(AccountService accountService, IMapper mapper,UserManager<AppUser> userManager)
        {
            _accountService = accountService;
            _mapper = mapper;
            _userManager = userManager;
        }
        #endregion

        #region common methods for child controllers
        [Authorize]
        [ApiExplorerSettings(IgnoreApi = true)]
        protected string GetUserId()
        {
            return _userManager.GetUserId(User);
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        #endregion

        #region virtual methods
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual string Create_BMs_ResourceUri(
            ResourceParameters _params,
            ResourceUriType resourceUriType,
            string routeName) 
        {
            switch (resourceUriType)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(routeName,
                    new 
                    {
                        PageNumber = _params.PageNumber - 1,
                        PageSize = _params.PageSize
                    });
                case ResourceUriType.NextPage:
                    return Url.Link(routeName,
                    new 
                    {
                        PageNumber = _params.PageNumber + 1,
                        PageSize = _params.PageSize
                    });
                default:
                    return Url.Link(routeName,
                    new 
                    {
                        PageNumber = _params.PageNumber,
                        PageSize = _params.PageSize
                    });
            }
        }
        #endregion
    }
}