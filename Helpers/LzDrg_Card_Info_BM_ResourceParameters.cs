using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys
{
    public class LzDrg_Card_Info_BM_ResourceParameters : ResourceParameters
    {
        private const int maxPageSize=10;
        private int _pageSize =10;
        public override int PageNumber { get;  set;} = 1;       
        public override int PageSize 
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
        public string S { get; set; }
        //cityIds=1,2,3&otherParama=...
        [ModelBinder(BinderType = typeof(ArrayModelBinder))] 
        public IEnumerable<byte> CityIds { get; set; }
        //areaIds=1,2,3&otherParama=...
        [ModelBinder(BinderType = typeof(ArrayModelBinder))]
        public IEnumerable<byte> AreaIds { get; set; }
        public string PhramId { get; set; }
        public DateTime ValidBefore { get; set; }=default(DateTime);
        public string OrderBy { get; set; } = "Name";
    }
}
