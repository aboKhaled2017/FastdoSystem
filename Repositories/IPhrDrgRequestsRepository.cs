using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;
using System_Back_End.Models;

namespace System_Back_End.Repositories
{
    public interface IPhrDrgRequestsRepository : IMainRepository
    {
         Task<PagedList<Made_PhDrgRequest_MB>> GetMadeRequestsByUser(LzDrReqResourceParameters _params);
         Task<PagedList<Sent_PhDrgRequest_MB>> GetSentRequestsForUser(LzDrReqResourceParameters _params);
         Task<PagedList<NotSeen_PhDrgRequest_MB>> GetNotSeenRequestsByUser(LzDrReqResourceParameters _params);
         Task<LzDrugRequest> GetByIdAsync(Guid id);
         void Add(LzDrugRequest lzDrugRequest);
         Task<LzDrugRequest> AddForUserAsync(Guid drugId);
         Task<bool> MakeRequestSeen(LzDrugRequest lzDrugRequest);
         void Delete(LzDrugRequest lzDrugRequest);
         Task<LzDrugRequest> GetIfExists(Guid reqId);
         Task<LzDrugRequest> GetRquestIfExistsForUser(Guid reqId);
         Task<LzDrugRequest> GetSentRquestIfExistsForUser(Guid reqId);
    }
}
