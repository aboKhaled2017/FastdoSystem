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
using Fastdo.API.Services;
using Fastdo.API.Services.Auth;
using Fastdo.Core;
using Fastdo.Core.Services;
using Fastdo.Core.Services.Auth;

namespace Fastdo.API.Controllers
{
    public class SharedAPIController : Controller
    {
        #region constructor and properties
        protected readonly UserManager<AppUser> _userManager;
        protected readonly IEmailSender _emailSender;
        protected readonly IAccountService _accountService;
        protected readonly ITransactionService _transactionService;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        public SharedAPIController(
            UserManager<AppUser> userManager,
            IEmailSender emailSender,
            IAccountService accountService,
            IMapper mapper,
            ITransactionService transactionService,
            IUnitOfWork unitOfWork)
            :this(accountService,mapper,userManager)
        {           
            _emailSender = emailSender;     
            _transactionService = transactionService;
            _unitOfWork = unitOfWork;
        }

        public SharedAPIController(IAccountService accountService, IMapper mapper,UserManager<AppUser> userManager)
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
            IResourceParameters _params,
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