using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.API.Repositories;
using Fastdo.Core;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fastdo.API.Areas.AdminPanel.Controllers
{
    [Authorize(Policy = "ControlOnVStockPagePolicy")]
    public class VStockController : MainController
    {
        public VStockController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager) : base(unitOfWork, userManager)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}