using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.backendsys.Models;

namespace Fastdo.backendsys.Repositories
{
    public class LzDrgRequestsRepository : Repository<LzDrugRequest>,ILzDrgRequestsRepository
    {
        public ILzDrugRepository _LzDrugRepository { get; }
        public LzDrgRequestsRepository(SysDbContext context,ILzDrugRepository lzDrugRepository) : base(context)
        {
            _LzDrugRepository = lzDrugRepository;
        }
        public async Task<PagedList<Made_LzDrgRequest_MB>> Get_AllRequests_I_Made(LzDrgReqResourceParameters _params)
        {
            var items = GetAll()
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
            var items = GetAll()
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
            var items = GetAll()
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
            return await GetAll()
                .Where(r => r.LzDrug.PharmacyId == UserId && reqIds.Contains(r.Id)).ToListAsync();
        }
        public async Task<object> GetByIdAsync(Guid id)
        {
            return await GetAll().Select(r=>new { 
              r.Id,
              r.LzDrugId,
              r.PharmacyId,
              r.Seen,
              r.Status
            }).FirstOrDefaultAsync(r=>r.Id==id);
        }

        public async Task<LzDrugRequest> AddForUserAsync(Guid drugId)
        {
            if (await _LzDrugRepository.GetAll()
                .AnyAsync(d => d.Id == drugId&&d.PharmacyId== UserId))
                return null;
            if (await GetAll()
                .AnyAsync(r => r.PharmacyId == UserId && r.LzDrugId == drugId))
                return null;
            var req = new LzDrugRequest
            {
                PharmacyId = UserId,
                LzDrugId = drugId
            };
            await AddAsync(req);
            return req;
        }
        public async Task<bool> Patch_Update_Request_Sync(LzDrugRequest lzDrugRequest)
        {
            return await UpdateFieldsAsync_And_Save(lzDrugRequest, prop => prop.Seen, prop => prop.Status);
        }
        public void Patch_Update_Group_Of_Requests_Sync(IEnumerable<LzDrugRequest> lzDrugRequests)
        {
            lzDrugRequests.ToList().ForEach(req =>
            {
                UpdateFields(req, prop => prop.Seen, prop => prop.Status);
            });
        }
        public async Task<bool> Make_RequestSeen(LzDrugRequest lzDrugRequest)
        {
            lzDrugRequest.Seen = true;
            return await UpdateFieldsAsync_And_Save(lzDrugRequest, prop => prop.Seen);
        }

        public void Delete(LzDrugRequest lzDrugRequest)
        {
            Remove(lzDrugRequest);
        }
        public void Delete_AllRequests_I_Made()
        {
            var reqs = GetAll().Where(r => r.PharmacyId == UserId);
            RemoveRange(reqs);
        }
        public void Delete_SomeRequests_I_Made(IEnumerable<Guid> Ids)
        {
            var reqs = GetAll().Where(r =>Ids.Contains(r.Id));
            RemoveRange(reqs);
        }
        public async Task<bool> User_Made_These_Requests(IEnumerable<Guid> Ids)
        {
            return (await GetAll()
                .CountAsync(r =>r.PharmacyId==UserId && Ids.Contains(r.Id))) == Ids.Count();
        }
        public async Task<bool> User_Received_These_Requests(IEnumerable<Guid> Ids)
        {
            return (await GetAll()
                .CountAsync(r => r.LzDrug.PharmacyId == UserId && Ids.Contains(r.Id))) == Ids.Count();
        }

        public async Task<LzDrugRequest> Get_Request_I_Made_IfExistsForUser(Guid reqId)
        {
            return await GetAll().FirstOrDefaultAsync(r=>r.Id== reqId && r.PharmacyId== UserId);
        }
        public async Task<LzDrugRequest> Get_Request_I_Received_IfExistsForUser(Guid reqId)
        {
            return await GetAll().FirstOrDefaultAsync(r => r.Id == reqId && r.LzDrug.PharmacyId == UserId);
        }

        public async Task<PagedList<Show_LzDrgsReq_ADM_Model>> GET_PageOf_LzDrgsRequests(LzDrgReqResourceParameters _params)
        {
            var items = GetAll();
               
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
