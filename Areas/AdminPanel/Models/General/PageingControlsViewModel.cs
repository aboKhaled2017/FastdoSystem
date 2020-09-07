using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Areas.AdminPanel.Models
{
    public class PageingControlsViewModel
    {
        public string SearchPlaceholder { get; set; }
        public bool RequireSearch { get; set; } = true;
        public IHtmlContent OtherSections { get; set; }
    }
}
