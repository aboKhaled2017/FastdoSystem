using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Models
{
    public class UpdatePhNameModel
    {
        [Required(ErrorMessage = "الاسم مطلوب")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "الاسم مابين 5 الى 20 حرف")]
        public string NewName { get; set; }
    }
    public class UpdateStkNameModel
    {
        [Required(ErrorMessage = "الاسم مطلوب")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "الاسم مابين 5 الى 20 حرف")]
        public string NewName { get; set; }
    }
}
