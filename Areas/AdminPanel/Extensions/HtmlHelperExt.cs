
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc.Rendering
{
    public static class HtmlHelperExt
    { 
        public static IHtmlContent FormateTextToElementIfZero(this IHtmlHelper helper,string elName,int val,string replaceText,string elClass = "")
        {
            if (val != 0) return new HtmlString(val.ToString());
            
            return new HtmlString(string.Format("<{0} class=\"{1}\">{2}</{0}>", elName,elClass,replaceText));
        }
    }
}
