using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;
using System_Back_End.Models;
using System_Back_End.Services;

namespace System_Back_End.Repositories
{
    public class LzDrugRepository:MainRepository
    {     

        public LzDrugRepository(SysDbContext context) : base(context)
        {
        }

        public void Add(LzDrug drug)
        {
            drug.PharmacyId = PharmacyId;
            _context.LzDrugs.Add(drug);
        }
        public DbSet<LzDrug> GetAllAsync()
        {
            return _context.LzDrugs;
        }
        public IQueryable<object> GetAllShown()
        {
            return GetAllAsync().Select(d => new ShowLzDrugModel 
            {
                Id=d.Id,
                Name=d.Name,
                Price=d.Price,
                PriceType=d.PriceType,
                Quantity=d.Quantity,
                Type=d.Type,
                UnitType=d.UnitType,
                ValideDate=d.ValideDate,
                ConsumeType=d.ConsumeType,
                Discount=d.Discount,
                Desc=d.Desc
            });
        }
        public IQueryable<object> GetAllShownExcept(string userId)
        {
            return GetAllAsync()
            .Where(d=>d.PharmacyId!=userId)
            .Select(d => new ShowLzDrugModel
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
        }
        public IQueryable<ShowLzDrugModel> GetAllShownOfUser(string userId)
        {
            return GetAllAsync()
            .Where(d => d.PharmacyId == userId)
            .Select(d => new ShowLzDrugModel
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
        }
        public void Update(LzDrug drug)
        {
            drug.PharmacyId = PharmacyId;
            _context.Entry(drug).State = EntityState.Modified;

        }
        public async Task<ShowLzDrugModel> GetShownModelByIdAsync(Guid id)
        {
            return await _context.LzDrugs
                .Where(d=>d.Id==id)
                .Select(d=>new ShowLzDrugModel {
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
        public async Task<LzDrug>GetByIdAsync(Guid id)
        {
            return await _context.LzDrugs.FindAsync(id);
        }
        public void Delete(LzDrug drug)
        {
            _context.LzDrugs.Remove(drug);
        }
        public async Task<bool> IsUserHas(Guid id)
        {
            return await _context.LzDrugs.AnyAsync(d => d.Id == id && d.PharmacyId==PharmacyId);
        }
        public async Task<LzDrug> GetIfExists(Guid id)
        {
            return await _context.LzDrugs.FindAsync(id);
        }
        public async Task<bool> LzDrugExists(Guid id)
        {
            return await _context.LzDrugs.AnyAsync(d=>d.Id==id);
        }

    }
}

