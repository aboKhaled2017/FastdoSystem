using Fastdo.Core.Enums;

namespace Fastdo.backendsys
{
    public class PharmaReqsResourceParameters : ResourceParameters
    {
        private const int maxPageSize = 10;
        private int _pageSize = 10;
        public override int PageNumber { get; set; } = 1;
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
        public PharmacyRequestStatus? Status { get; set; } = null;
        public string PharmaClass { get; set; } 
    }
}
