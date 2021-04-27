using Fastdo.Core.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.API.Services.Auth
{
    public interface IAccountService : IBastrctAccountService
    {
        void SetCurrentContext(HttpContext httpContext, IUrlHelper url);
    }
}
