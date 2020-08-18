using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.backendsys.Services;
using System.Linq.Dynamic.Core;
namespace Fastdo.backendsys
{
    public static class IQuerableExtensions
    {
        public static IQueryable<T> ApplySort<T>(
            this IQueryable<T>source ,
            string orderBy, 
            IDictionary<string,PropertyMappingValue> mappingDictionary)
        {
            if (source == null)
                throw new ArgumentException("source");
            if (mappingDictionary == null)
                throw new ArgumentException("mappingDictionary");
            if (string.IsNullOrWhiteSpace(orderBy))
                return source;
            //orderby string is seperated
            var orderAfterSplit = orderBy.Split(",");

            //apply each orderby clause in revers order -otherwise,the
            //IQuerable will be ordered in the wrong order
            foreach (var orderbyClause in orderAfterSplit.Reverse())
            {
                var trimmedOrderbyClause = orderbyClause.Trim();
                var isOrderDescending = trimmedOrderbyClause.EndsWith(" desc");

                //remove " asc" or " desc" from the orderbyclause
                //and get the propertyName to look for in the mapping dictionary
                var indexOfTheFirstSpace = trimmedOrderbyClause.IndexOf(" ");
                var propertName = indexOfTheFirstSpace == -1
                    ? trimmedOrderbyClause : trimmedOrderbyClause.Remove(indexOfTheFirstSpace);

                //finding the matching property
                if (!mappingDictionary.ContainsKey(propertName))
                    throw new ArgumentException($"key mapping for {propertName} is missing");

                //get the property mapping value
                var propertyMappingValue = mappingDictionary[propertName];
                if (propertyMappingValue==null)
                    throw new ArgumentNullException("propertyMappingValue");

                //run throught the property name in revers
                //so the orderby clauses are applied in the correct order
                foreach (var destinationProp in propertyMappingValue.DestinationProperties.Reverse())
                {
                    //revert sort order if necessary
                    if (propertyMappingValue.Revert)
                        isOrderDescending = !isOrderDescending;
                    source = source.OrderBy(destinationProp + (isOrderDescending ? " descending" : " ascending"));
                }
            }
            return source;
        }
    }
}
