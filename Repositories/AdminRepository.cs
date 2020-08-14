using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Enums;
using System.Linq;
using System.Models;
using System.Threading.Tasks;
using System_Back_End.Models;

namespace System_Back_End.Repositories
{
    public class AdminRepository : MainRepository, IAdminRepository
    {
        public AdminRepository(SysDbContext context) : base(context)
        {
        }

        public async Task<Admin> GetByIdAsync(string id)
        {
            return await _context.Admins.FindAsync(id);
        }
        public async Task<ShowAdminModel> GetAdminsShownModelById(string id)
        {
            return await GetAllAdminsShownModels()
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
        }
        public IQueryable<Admin> GetAllAdmins()
        {
            return _context.Admins.AsQueryable();
        }
        public IQueryable<ShowAdminModel> GetAllAdminsShownModels(string adminType = null)
        {
            var data=GetAllAdmins().Select(a=>new ShowAdminModel
            { 
                Id=a.Id,
                Name=a.Name,
                SuperId=a.SuperAdminId,
                UserName=a.User.UserName,
                PhoneNumber=a.User.PhoneNumber,
                Type=_context.UserClaims.FirstOrDefault(c=>c.UserId==a.Id &&c.ClaimType==Variables.AdminClaimsTypes.AdminType).ClaimValue,
                Prevligs = _context.UserClaims.FirstOrDefault(c => c.UserId == a.Id && c.ClaimType == Variables.AdminClaimsTypes.Previligs).ClaimValue
            });
            if (adminType != null)
                data = data.Where(d => d.Type == adminType);
            return data;
        }
        public async Task<bool> AddAsync(Admin admin)
        {
            _context.Admins.Add(admin);
            _context.AdminHistoryEntries.Add(new AdminHistory
            {
                IssuerId = UserId,
                OperationType = "Insert",
                ToId = admin.Id,
                Desc = $"the user {UserName} with id {UserId} inserted new admin [{admin.Name}]"
            });
            return (await _context.SaveChangesAsync()) > 0;
        }
        public async Task<StatisShowModel> GetGeneralStatisOfSystem()
        {           
            var queryResult = _context.Pharmacies.Select(d=>new StatisShowModel
            { 
                pharmacies=new PharmaciesStatis
                {
                 total=_context.Pharmacies.Count(),
                 disabled= _context.Pharmacies.Count(p=>p.Status==PharmacyRequestStatus.Disabled),
                 pending = _context.Pharmacies.Count(p => p.Status == PharmacyRequestStatus.Pending),
                 accepted = _context.Pharmacies.Count(p => p.Status == PharmacyRequestStatus.Accepted),
                 rejected = _context.Pharmacies.Count(p => p.Status == PharmacyRequestStatus.Rejected)
                },
                stocks = new StocksStatis
                {
                    total = _context.Stocks.Count(),
                    disabled = _context.Stocks.Count(p => p.Status == StockRequestStatus.Disabled),
                    pending = _context.Stocks.Count(p => p.Status == StockRequestStatus.Pending),
                    accepted = _context.Stocks.Count(p => p.Status == StockRequestStatus.Accepted),
                    rejected = _context.Stocks.Count(p => p.Status == StockRequestStatus.Rejected)
                },
                requests=new RequestsStatis
                {
                    total = _context.LzDrugRequests.Count(),
                    done = _context.LzDrugRequests.Count(s=>s.Status==LzDrugRequestStatus.Completed),
                    pending = _context.LzDrugRequests.Count(s => s.Status == LzDrugRequestStatus.Pending),
                    cancel = _context.LzDrugRequests.Count(s => s.Status == LzDrugRequestStatus.Rejected)
                }
            });
            return await queryResult.FirstAsync();
        }
        public void Update(Admin admin)
        {
            _context.AdminHistoryEntries.Add(new AdminHistory
            {
                IssuerId = UserId,
                OperationType = "Update",
                ToId = admin.Id,
                Desc = $"the user {UserName} with id {UserId} Updated admin [{admin.Name}]"
            });
            _context.Entry(admin).State = EntityState.Modified;

        }
        public void Delete(Admin admin)
        {
            var subAdmins = _context.Admins.Where(a => a.SuperAdminId == admin.Id).ToList();
            foreach (var _subAdmin in subAdmins)
            {
                _subAdmin.SuperAdminId = admin.SuperAdminId;
                UpdateFields<Admin>(_subAdmin, a => a.SuperAdminId);               
            }
            if(subAdmins.Count>0) _context.SaveChanges();
            _context.AdminHistoryEntries.Add(new AdminHistory
            {
                IssuerId = UserId,
                OperationType = "Delete",
                ToId = admin.Id,
                Desc = $"the user {UserName} with id {UserId} deleted  admin [{admin.Name}]"
            });
            _context.Admins.Remove(admin);
        }
    }
}
