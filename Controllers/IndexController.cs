using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace System_Back_End.Controllers
{
    [Route("api")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly SysDbContext _context;
        public IndexController(SysDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult get()
        {

            return Ok(_context.LzDrugs.ToList());
        }
    }
}