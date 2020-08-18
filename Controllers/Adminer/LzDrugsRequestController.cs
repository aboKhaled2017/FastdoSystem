using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fastdo.backendsys.Controllers.Adminer
{
    public class LzDrugsRequestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}