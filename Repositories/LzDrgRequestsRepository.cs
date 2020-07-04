using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;
using System_Back_End.Models;

namespace System_Back_End.Repositories
{
    public class LzDrgRequestsRepository : MainRepository,ILzDrgRequestsRepository
    {
        public LzDrgRequestsRepository(SysDbContext context) : base(context)
        {
        }
        public async Task<PagedList<Made_LzDrgRequest_MB>> Get_AllRequests_I_Made(LzDrgReqResourceParameters _params)
        {
            var items = _context.LzDrugRequests
                .Where(d => d.PharmacyId == UserId)
                .Select(r => new Made_LzDrgRequest_MB
                {
                    Id=r.Id,
                    LzDrugId=r.LzDrugId,
                    Status=r.Status,
                    PharmacyId=r.LzDrug.PharmacyId,
                    PhName=r.LzDrug.Pharmacy.Name
                });
            return await PagedList<Made_LzDrgRequest_MB>.CreateAsync(items, _params);
        }
        public async Task<PagedList<Sent_LzDrgRequest_MB>> Get_AllRequests_I_Received(LzDrgReqResourceParameters _params)
        {
            var items = _context.LzDrugRequests
                .Where(d => d.LzDrug.PharmacyId == UserId)
                .Select(r => new Sent_LzDrgRequest_MB
                {
                    Id = r.Id,
                    LzDrugId = r.LzDrugId,
                    Status = r.Status,
                    PharmacyId = r.PharmacyId,
                    PhName = r.Pharmacy.Name
                });
            return await PagedList<Sent_LzDrgRequest_MB>.CreateAsync(items, _params);
        }
        public async Task<PagedList<NotSeen_PhDrgRequest_MB>> Get_AllRequests_I_Received_INS(LzDrgReqResourceParameters _params)
        {
            var items = _context.LzDrugRequests
                .Where(r => r.LzDrug.PharmacyId == UserId&&!r.Seen)
                .Select(r => new NotSeen_PhDrgRequest_MB
                {
                    Id = r.Id,
                    LzDrugId = r.LzDrugId,
                    Status = r.Status,
                    PharmacyId = r.PharmacyId,
                    PhName = r.Pharmacy.Name
                });
            return await PagedList<NotSeen_PhDrgRequest_MB>.CreateAsync(items, _params);
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
        public async Task<bool> Patch_UpdateSync(LzDrugRequest lzDrugRequest)
        {
            return await UpdateFieldsAsync_And_Save<LzDrugRequest>(lzDrugRequest, prop => prop.Seen, prop => prop.Status);
        }
        public async Task<bool> Make_RequestSeen(LzDrugRequest lzDrugRequest)
        {
            lzDrugRequest.Seen = true;
            return await UpdateFieldsAsync_And_Save<LzDrugRequest>(lzDrugRequest, prop => prop.Seen);
        }
        public void Delete(LzDrugRequest lzDrugRequest)
        {
            _context.LzDrugRequests.Remove(lzDrugRequest);
        }
        public async Task<LzDrugRequest> Get_IfExists(Guid reqId)
        {
            return await _context.LzDrugRequests.FindAsync(reqId);
        }
        public async Task<LzDrugRequest> Get_Request_I_Made_IfExistsForUser(Guid reqId)
        {
            return await _context.LzDrugRequests.FirstOrDefaultAsync(r=>r.Id== reqId && r.PharmacyId== UserId);
        }
        public async Task<LzDrugRequest> Get_Request_I_Received_IfExistsForUser(Guid reqId)
        {
            return await _context.LzDrugRequests.FirstOrDefaultAsync(r => r.Id == reqId && r.LzDrug.PharmacyId == UserId);
        }

    }
}
