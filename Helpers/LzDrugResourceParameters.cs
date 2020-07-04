using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System_Back_End
{
    public class LzDrugResourceParameters
    {
        private const int maxPageSize=10;
        private int _pageSize =10;
        public int PageNumber { get;  set;} = 1;       
        public int PageSize 
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
    }
}
