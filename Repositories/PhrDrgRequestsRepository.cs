using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;

namespace System_Back_End.Repositories
{
    public class PhrDrgRequestsRepository : MainRepository
    {
        public PhrDrgRequestsRepository(SysDbContext context) : base(context)
        {
        }
        public IQueryable<LzDrugRequest> GetMadeRequestsByUser()
        {
            return _context.LzDrugRequests.Where(d => d.PharmacyId == UserId);
        }
        public IQueryable<LzDrugRequest> GetSentRequestsForUser()
        {
            return _context.LzDrugRequests.Where(d =>d.LzDrug.PharmacyId==UserId);
        }
        public IQueryable<LzDrugRequest> GetNotSeenRequestsByUser()
        {
            return GetSentRequestsForUser().Where(r => !r.Seen);
        }
        public IQueryable<LzDrugRequest> GetForLzDrug(Guid drugId)
        {
            return _context.LzDrugRequests.Where(d => d.LzDrugId == drugId);
        }
        public async Task<LzDrugRequest> GetByIdAsync(Guid id)
        {
            return await _context.LzDrugRequests.FindAsync(id);
        }
        public void Add(LzDrugRequest lzDrugRequest)
        {
            _context.LzDrugRequests.Add(lzDrugRequest);
        }
        public async Task<LzDrugRequest> AddForUserAsync(Guid drugId)
        {
            if (await _context.LzDrugs.AnyAsync(d => d.Id == drugId&&d.PharmacyId== UserId))
                return null;
            if (await _context.LzDrugRequests.AnyAsync(r => r.PharmacyId == UserId && r.LzDrugId == drugId))
                return null;
            var req = new LzDrugRequest
            {
                PharmacyId = UserId,
                LzDrugId = drugId
            };
            _context.LzDrugRequests.Add(req);
            return req;
        }
        public async Task<bool> MakeRequestSeen(LzDrugRequest lzDrugRequest)
        {
            lzDrugRequest.Seen = true;
            return await UpdateFields<LzDrugRequest>(lzDrugRequest, prop => prop.Seen);
        }
        public void Delete(LzDrugRequest lzDrugRequest)
        {
            _context.LzDrugRequests.Remove(lzDrugRequest);
        }
        public async Task<LzDrugRequest>GetIfExists(Guid reqId)
        {
            return await _context.LzDrugRequests.FindAsync(reqId);
        }
        public async Task<LzDrugRequest>GetRquestIfExistsForUser(Guid reqId)
        {
            return await _context.LzDrugRequests.FirstOrDefaultAsync(r=>r.Id== reqId && r.PharmacyId== UserId);
        }
        public async Task<LzDrugRequest> GetSentRquestIfExistsForUser(Guid reqId)
        {
            return await _context.LzDrugRequests.FirstOrDefaultAsync(r => r.Id == reqId && r.LzDrug.PharmacyId == UserId);
        }
    }
}
