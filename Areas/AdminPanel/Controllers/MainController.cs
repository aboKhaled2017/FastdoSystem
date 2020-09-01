using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fastdo.backendsys.Areas.AdminPanel.Controllers
{
    [Authorize(policy: "AdminPolicy")]
    [Area("AdminPanel")]
    public class MainController : Controller
    {
        
    }
}