using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fastdo.Core.Models
{
    public class AppUser:IdentityUser
    {
        [MaxLength(15)]
        public string confirmCode { get; set; }
        [EmailAddress]
        public string willBeNewEmail { get; set; }
    }
}
