using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys
{
    public class PaginationMetaData
    {
        public int totalCount { get; set; }
        public int pageSize { get; set; }
        public int currentPage { get; set; }
        public int totalPages { get; set; }
        public string prevPageLink { get; set; }
        public string nextPageLink { get; set; }
    }
}
