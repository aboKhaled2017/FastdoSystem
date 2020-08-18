using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys
{
    public class PaginationMetaDataGenerator<T,P>
    {
        private PagedList<T> Items { get; set; }
        private string RouteName { get; set; }
        private P Parameters{ get; set; }
        private Func<P, ResourceUriType,string, string> GetLinkForResource { get; set; }

        public PaginationMetaDataGenerator(
            PagedList<T> items,
            string routeName,
            P parameters,
            Func<P, ResourceUriType, string, string> getLinkForResource)
        {
            Items = items;
            RouteName = routeName;
            GetLinkForResource = getLinkForResource;
            Parameters = parameters;
        }
        public string Generate()
        {
            var paginationMetaData = new PaginationMetaData
            {
                totalCount = Items.TotalCount,
                pageSize = Items.PageSize,
                currentPage = Items.CurrentPage,
                totalPages = Items.TotalPages,
                prevPageLink =Items.HasPrevious
                ?GetLinkForResource.Invoke(Parameters,ResourceUriType.PreviousPage,RouteName)
                :null,
                nextPageLink =Items.HasNext
                ?GetLinkForResource.Invoke(Parameters, ResourceUriType.NextPage, RouteName)
                :null
            };
            return JsonConvert.SerializeObject(paginationMetaData);
        }
    }
}
