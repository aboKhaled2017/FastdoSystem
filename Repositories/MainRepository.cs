using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;

namespace System_Back_End.Repositories
{
    public class MainRepository
    {
        public SysDbContext _context { get; }
        public MainRepository(SysDbContext context)
        {
            _context = context;
        }
    }
}
