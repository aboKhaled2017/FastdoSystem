using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys
{
    public class LzDrgResourceParameters: ResourceParameters
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
    }
}
