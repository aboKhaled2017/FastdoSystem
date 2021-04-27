using Fastdo.Core.Services;
using Fastdo.Core.Services.UserPropertyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.API.Services
{
    public class PropertyMapping<TSource,TDestination>: IPropertyMapping
    {
        public Dictionary<string,IPropertyMappingValue> _mappingDictionary { get; private set; }
        public PropertyMapping(Dictionary<string,IPropertyMappingValue> mappingDictionary)
        {
            _mappingDictionary = mappingDictionary;
        }
    }
}
