using Fastdo.Core.Enums;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Graphql
{
    public class StockRequestStatusType:EnumerationGraphType<StockRequestStatus>
    {
        public StockRequestStatusType()
        {
            Name = "Status";
            Description = "status of its request to fastdo";
        }
    }
}
