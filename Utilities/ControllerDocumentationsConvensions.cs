using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Fastdo.Core.Utilities
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
