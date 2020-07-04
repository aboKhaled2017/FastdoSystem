using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;
using System_Back_End.Models;

namespace System_Back_End.Repositories
{
    public interface ILzDrgRequestsRepository : IMainRepository
    {
         Task<PagedList<Made_LzDrgRequest_MB>> GetMadeRequestsByUser(LzDrgReqResourceParameters _params);
         Task<PagedList<Sent_LzDrgRequest_MB>> GetSentRequestsForUser(LzDrgReqResourceParameters _params);
         Task<PagedList<NotSeen_PhDrgRequest_MB>> GetNotSeenRequestsByUser(LzDrgReqResourceParameters _params);
         Task<LzDrugRequest> GetByIdAsync(Guid id);
         void Add(LzDrugRequest lzDrugRequest);
         Task<LzDrugRequest> AddForUserAsync(Guid drugId);
         Task<bool> MakeRequestSeen(LzDrugRequest lzDrugRequest);
         void Delete(LzDrugRequest lzDrugRequest);
         Task<LzDrugRequest> GetIfExists(Guid reqId);
         Task<LzDrugRequest> GetMadeRquestIfExistsForUser(Guid reqId);
         Task<LzDrugRequest> GetSentRquestIfExistsForUser(Guid reqId);
    }
}
