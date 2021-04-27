using System;
using System.Collections.Generic;
using System.Text;

namespace Fastdo.Core.Services.UserPropertyService
{
    public interface IPropertyMappingValue
    {
        IEnumerable<string> DestinationProperties { get; }
        bool Revert { get; }
    }
}
