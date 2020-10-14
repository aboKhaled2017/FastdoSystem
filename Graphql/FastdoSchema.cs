using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Fastdo.backendsys.Graphql
{
    public class FastdoSchema:Schema
    {
        public FastdoSchema(IDependencyResolver resolver)  :base(resolver)    
        {
            Query = resolver.Resolve <FastdoQuery>();
        }
}
}
