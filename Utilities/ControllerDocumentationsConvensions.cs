using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Utilities
{
    public class ControllerDocumentationsConvensions : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller == null) return;
            foreach (var attrib in controller.Attributes)
            {
                if (attrib.GetType() == typeof(RouteAttribute))
                {
                    var routeAttrib = (RouteAttribute)attrib;
                    if (string.IsNullOrEmpty(routeAttrib.Name) == false)
                        controller.ControllerName = routeAttrib.Name;
                }
            }
        }
    }
}
