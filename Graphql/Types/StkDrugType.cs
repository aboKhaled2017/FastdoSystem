using Fastdo.API.Repositories;
using Fastdo.Core.Models;
using Fastdo.Core.Repositories;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.API.Graphql
{
    public class StkDrugType:ObjectGraphType<StkDrug>
    {
        public StkDrugType(IStockRepository stockRepository)
        {
            Field(x=>x.Id,type:typeof(IdGraphType));
            Field(x => x.Name);
            Field(x => x.Price);
            Field(x => x.StockId).Description("stock id if the drug");
            Field(x => x.Discount);   
            Field(x => x.LastUpdate);

            Field<StockType>("stock", 
                resolve: context => stockRepository.GetById(context.Source.StockId));
        }
    }
}
