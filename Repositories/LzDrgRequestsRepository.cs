using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Repositories.Models;
using System.Threading.Tasks;
using Fastdo.backendsys.Models;

namespace Fastdo.backendsys.Repositories
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
                    LzDrugName=r.LzDrug.Name,
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
                    LzDrugName=r.LzDrug.Name,
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
                    LzDrugName=r.LzDrug.Name,
                    Status = r.Status,
                    PharmacyId = r.PharmacyId,
                    PhName = r.Pharmacy.Name
                });
            return await PagedList<NotSeen_PhDrgRequest_MB>.CreateAsync(items, _params);
        }
        public async Task<IEnumerable<LzDrugRequest>> Get_Group_Of_Requests_I_Received(IEnumerable<Guid> reqIds)
        {
            return await _context.LzDrugRequests
                .Where(r => r.LzDrug.PharmacyId == UserId && reqIds.Contains(r.Id)).ToListAsync();
        }
        public async Task<object> GetByIdAsync(Guid id)
        {
            return await _context.LzDrugRequests.Select(r=>new { 
              r.Id,
              r.LzDrugId,
              r.PharmacyId,
              r.Seen,
              r.Status
            }).FirstOrDefaultAsync(r=>r.Id==id);
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
        public async Task<bool> Patch_Update_Request_Sync(LzDrugRequest lzDrugRequest)
        {
            return await UpdateFieldsAsync_And_Save<LzDrugRequest>(lzDrugRequest, prop => prop.Seen, prop => prop.Status);
        }
        public void Patch_Update_Group_Of_Requests_Sync(IEnumerable<LzDrugRequest> lzDrugRequests)
        {
            lzDrugRequests.ToList().ForEach(req =>
            {
                UpdateFields<LzDrugRequest>(req, prop => prop.Seen, prop => prop.Status);
            });
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
        public void Delete_AllRequests_I_Made()
        {
            var reqs = _context.LzDrugRequests.Where(r => r.PharmacyId == UserId);
            _context.LzDrugRequests.RemoveRange(reqs);
        }
        public void Delete_SomeRequests_I_Made(IEnumerable<Guid> Ids)
        {
            var reqs = _context.LzDrugRequests.Where(r =>Ids.Contains(r.Id));
            _context.LzDrugRequests.RemoveRange(reqs);
        }
        public async Task<bool> User_Made_These_Requests(IEnumerable<Guid> Ids)
        {
            return (await _context.LzDrugRequests
                .CountAsync(r =>r.PharmacyId==UserId && Ids.Contains(r.Id))) == Ids.Count();
        }
        public async Task<bool> User_Received_These_Requests(IEnumerable<Guid> Ids)
        {
            return (await _context.LzDrugRequests
                .CountAsync(r => r.LzDrug.PharmacyId == UserId && Ids.Contains(r.Id))) == Ids.Count();
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

        public async Task<PagedList<Show_LzDrgsReq_ADM_Model>> GET_PageOf_LzDrgsRequests(LzDrgReqResourceParameters _params)
        {
            var items = _context.LzDrugRequests.AsQueryable();
               
            if (_params.Seen != null)
            {
                items = items.Where(i => i.Seen == _params.Seen);
            }
            if (_params.Status != null)
            {
                items = items.Where(i => i.Status == _params.Status);
            }
            var data=items
                .Select(r => new Show_LzDrgsReq_ADM_Model
                {
                    Id = r.Id,
                    LzDrugId = r.LzDrugId,
                    LzDrugName = r.LzDrug.Name,
                    Status = r.Status,
                    OwenerPh_Id = r.LzDrug.PharmacyId,
                    OwenerPh_Name = r.LzDrug.Pharmacy.Name,
                    RequesterPhram_Id = r.PharmacyId,
                    RequesterPhram_Name = r.Pharmacy.Name
                });
            return await PagedList<Show_LzDrgsReq_ADM_Model>.CreateAsync(data, _params);
        }
    }
}
