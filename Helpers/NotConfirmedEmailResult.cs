using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys
{
    public class NotConfirmedEmailResult:ObjectResult
    {
        public NotConfirmedEmailResult(string err="email is not confirmed")
        :base(err)
        {
            StatusCode = 410;
        }
    }
}
