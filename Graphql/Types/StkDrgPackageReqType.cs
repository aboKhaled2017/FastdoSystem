using Fastdo.Core.Models;
using GraphQL.Instrumentation;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Graphql
{
    public class StkDrgPackageReqType:ObjectGraphType<StkDrugPackageRequest>
    {
        public StkDrgPackageReqType()
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("package request ID");
            Field(x => x.Name);
            Field(x => x.PharmacyId, type: typeof(IdGraphType));
        }
    }
}
