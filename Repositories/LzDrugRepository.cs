using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.backendsys.Models;
using Fastdo.backendsys.Services;

namespace Fastdo.backendsys.Repositories
{
    public class LzDrugRepository:Repository<LzDrug>,ILzDrugRepository
    {
        public LzDrugRepository(SysDbContext context) : base(context)
        {
        }

        public async Task<PagedList<Show_VStock_LzDrg_ADM_Model>> GET_PageOf_VStock_LzDrgs(LzDrgResourceParameters _params)
        {
            var sourceData = GetAll()
                        .OrderBy(d => d.Name)
                        .GroupBy(d =>new {d.Name,d.Type})                       
                        .Select(g => new Show_VStock_LzDrg_ADM_Model { 
                         Name=g.Key.Name,
                         Type=g.Key.Type,
                         Products=g.Select(d=>new VStock_LzDrg_For_Pharmacy_ADM_Model { 
                            DrugId=d.Id,
                            ConsumeType=d.ConsumeType,
                            Discount=d.Discount,
                            PharmacyId=d.PharmacyId,
                            PharmacyName=d.Pharmacy.Name,
                            Price=d.Price,
                            PriceType=d.PriceType,
                            Quantity=d.Quantity,
                            Type=d.Type,
                            UnitType=d.UnitType,
                            ValideDate=d.ValideDate,
                            Desc=d.Desc
                         })
                        });
            if (!string.IsNullOrEmpty(_params.S))
            {
                var searchQueryForWhereClause = _params.S.Trim().ToLowerInvariant();
                sourceData = sourceData
                     .Where(d => d.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }
            return await PagedList<Show_VStock_LzDrg_ADM_Model>.CreateAsync(sourceData, _params);
        }
        public async Task<PagedList<LzDrugModel_BM>> GetAll_BM(LzDrgResourceParameters _params)
        {

            var sourceData=GetAll()
            .Where(d => d.PharmacyId == UserId)
            .OrderBy(d=>d.Name)
            .Select(d => new LzDrugModel_BM
            {
                Id = d.Id,
                Name = d.Name,
                Price = d.Price,
                PriceType = d.PriceType,
                Quantity = d.Quantity,
                Type = d.Type,
                UnitType = d.UnitType,
                ValideDate = d.ValideDate,
                ConsumeType = d.ConsumeType,
                Discount = d.Discount,
                Desc = d.Desc,
                RequestCount=d.RequestingPharms.Count
            });
            return await PagedList<LzDrugModel_BM>.CreateAsync(sourceData, _params);
        }
        public async Task<LzDrugModel_BM> Get_BM_ByIdAsync(Guid id)
        {
            return await GetAll()
                .Where(d => d.Id == id)
                .Select(d => new LzDrugModel_BM
                {
                    Id = d.Id,
                    Name = d.Name,
                    Price = d.Price,
                    PriceType = d.PriceType,
                    Quantity = d.Quantity,
                    Type = d.Type,
                    UnitType = d.UnitType,
                    ValideDate = d.ValideDate,
                    ConsumeType = d.ConsumeType,
                    Discount = d.Discount,
                    Desc = d.Desc
                }).FirstOrDefaultAsync();
        }

        public override void Add(LzDrug model)
        {
            model.PharmacyId = UserId;
            model.Name = model.Name.Trim();
            base.Add(model);
        }
        public void Update(LzDrug drug)
        {
            drug.PharmacyId = UserId;
            _context.Entry(drug).State = EntityState.Modified;

        }

        public async Task<bool> IsUserHas(Guid id)
        {
            return await GetAll()
                .AnyAsync(d => d.Id == id && d.PharmacyId==UserId);
        }

        public async Task<bool> LzDrugExists(Guid id)
        {
            return await GetAll()
                .AnyAsync(d=>d.Id==id);
        }

        public async Task<Show_LzDrgReqDetails_ADM_Model> GEt_LzDrugDetails_For_ADM(Guid id)
        {
            return await GetAll()
                .Where(d => d.Id == id)
                .Select(d=>new Show_LzDrgReqDetails_ADM_Model
                {
                    Id=d.Id,
                    Desc=d.Desc,
                    Discount=d.Discount,
                    Name=d.Name,
                    Price=d.Price,
                    PriceType=d.PriceType,
                    Quantity=d.Quantity,
                    Type=d.Type,
                    UnitType=d.UnitType,
                    ValideDate=d.ValideDate,
                    RequestCount=d.RequestingPharms.Count
                })
                .SingleOrDefaultAsync();
        }

    }
}

