using Fastdo.backendsys.Repositories;
using Fastdo.Core.Enums;
using Fastdo.Core.Models;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Graphql
{

    public class StockType : ObjectGraphType<Stock>
    {
        public StockType(IStkDrugsRepository stkDrugsRepository, IDataLoaderContextAccessor dataLoader)
        {
            Field(x => x.Id,type:typeof(IdGraphType)).Description("Stock id");
            Field(x => x.Name).Description("Stock name.");
            Field(x => x.Address, nullable: true);
            Field(x => x.AreaId,type:typeof(IdGraphType));
            Field(x => x.OwnerName);
            Field(x => x.MgrName);
            Field(x => x.CommercialRegImgSrc);
            Field(x => x.LicenseImgSrc);
            Field(x => x.LandlinePhone);
            Field(x => x.PersPhone);
            Field<StockRequestStatusType>("Status");

            Field<ListGraphType<StkDrugType>>(
                "drugs",
                resolve: context =>
                {
                    var loader = dataLoader.Context.GetOrAddCollectionBatchLoader<string, StkDrug>("load", async (keys) =>
                    {
                        var drugs = await stkDrugsRepository
                        .Where(s => keys.Contains(s.StockId)).ToListAsync();
                        return drugs.ToLookup(s => s.StockId);
                    });
                    return loader.LoadAsync(context.Source.Id);
                }
            ) ;
        }
    }
}
