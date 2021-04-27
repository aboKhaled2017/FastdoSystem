using Fastdo.Core.Services.UserPropertyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.Core.Services
{
    public interface IpropertyMappingService
    {
        Dictionary<string, IPropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool validMappingExistsFor<TSource, TDestination>(string fields);
    }
}
