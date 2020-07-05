using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;
using System_Back_End.Models;

namespace System_Back_End.Services
{
    public class PropertyMappingService: IpropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _lzDrugCard_Info_BM_PropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Name",new PropertyMappingValue(new List<string>{ "Name"})},
                {"Quantity",new PropertyMappingValue(new List<string>{ "Quantity"})},
                {"Price",new PropertyMappingValue(new List<string>{ "Price"})},
                {"ValideDate",new PropertyMappingValue(new List<string>{ "ValideDate"})},
            };
        private IList<IPropertyMapping> propertyMappings=new List<IPropertyMapping>();
        public PropertyMappingService()
        {
            propertyMappings.Add(new PropertyMapping<LzDrugCard_Info_BM,LzDrug>(_lzDrugCard_Info_BM_PropertyMapping));
        }
        public Dictionary<string,PropertyMappingValue> GetPropertyMapping<TSource,TDestination>()
        {
            var matchingMapping = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();
            if (matchingMapping.Count() == 1)
                return matchingMapping.First()._mappingDictionary;
            throw new Exception(Functions.MakeError("mapping error").ToString());
        }
    }
}
