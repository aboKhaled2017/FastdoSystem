using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Fastdo.Core.Services;

namespace Fastdo.Core.Utilities
{
    [Authorize]
    [AttributeUsage(AttributeTargets.All)]
    public partial class RequireConfirmedEmail:ValidationAttribute
    {
 
    }
}
