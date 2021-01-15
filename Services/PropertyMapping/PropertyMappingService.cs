using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.backendsys.Models;

namespace Fastdo.backendsys.Services
{
    public class PropertyMappingService: IpropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _lzDrugCard_Info_BM_PropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Name",new PropertyMappingValue(new List<string>{ "Name"})},
                {"Quantity",new PropertyMappingValue(new List<string>{ "Quantity"})},
                {"Price",new PropertyMappingValue(new List<string>{ "Price"})},
                {"Discount",new PropertyMappingValue(new List<string>{ "Discount"})},
                {"ValideDate",new PropertyMappingValue(new List<string>{ "ValideDate"})},
                {"requestsCount",new PropertyMappingValue(new List<string>{"requestsCount"})}
            };
        private Dictionary<string, PropertyMappingValue> _pharmacyCard_Info_BM_PropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Name",new PropertyMappingValue(new List<string>{ "Name"})},
            };
        private IList<IPropertyMapping> propertyMappings=new List<IPropertyMapping>();
        public PropertyMappingService()
        {
            propertyMappings.Add(new PropertyMapping<LzDrugCard_Info_BM,LzDrug>(_lzDrugCard_Info_BM_PropertyMapping));
            propertyMappings.Add(new PropertyMapping<Get_PageOf_Pharmacies_ADMModel, Pharmacy>(_pharmacyCard_Info_BM_PropertyMapping));
        }
        public Dictionary<string,PropertyMappingValue> GetPropertyMapping<TSource,TDestination>()
        {
            var matchingMapping = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();
            if (matchingMapping.Count() == 1)
                return matchingMapping.First()._mappingDictionary;
            throw new Exception(Functions.MakeError("mapping error").ToString());
        }
        public bool validMappingExistsFor<TSource,TDestination>(string fields)
        {
            var propMapping = GetPropertyMapping<TSource, TDestination>();
            if (string.IsNullOrWhiteSpace(fields))
                return true;
            var fieldsAfterSplit = fields.Split(",");

            foreach (var field in fieldsAfterSplit)
            {
                var trimmedField = field.Trim();
                var indexOfFirstWhitespace = trimmedField.IndexOf(" ");

                var propertName = indexOfFirstWhitespace == -1
                    ? trimmedField : trimmedField.Remove(indexOfFirstWhitespace);

                //finding the matching property
                if (!propMapping.ContainsKey(propertName))
                    return false;
            }
            return true;
        }
    }
}
