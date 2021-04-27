using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Fastdo.Core.Services;
using Fastdo.Core.Enums;

namespace Fastdo.Core.Utilities
{
    [Authorize]
    public partial class CheckIfUserPropValueIsExixtsOnUpdate:ValidationAttribute
    {
       
    }
}
