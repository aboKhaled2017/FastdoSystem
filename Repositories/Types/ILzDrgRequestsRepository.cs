using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;
using System_Back_End.Models;

namespace System_Back_End.Repositories
{
    public interface ILzDrgRequestsRepository :IMainRepository
    {
         Task<PagedList<Made_LzDrgRequest_MB>> Get_AllRequests_I_Made(LzDrgReqResourceParameters _params);
         Task<PagedList<Sent_LzDrgRequest_MB>> Get_AllRequests_I_Received(LzDrgReqResourceParameters _params);
         Task<PagedList<NotSeen_PhDrgRequest_MB>> Get_AllRequests_I_Received_INS(LzDrgReqResourceParameters _params);
         Task<LzDrugRequest> GetByIdAsync(Guid id);
         void Add(LzDrugRequest lzDrugRequest);
         Task<LzDrugRequest> AddForUserAsync(Guid drugId);
         Task<bool> Patch_UpdateSync(LzDrugRequest lzDrugRequest);
         Task<bool> Make_RequestSeen(LzDrugRequest lzDrugRequest);
         void Delete(LzDrugRequest lzDrugRequest);
         Task<LzDrugRequest> Get_IfExists(Guid reqId);
         Task<LzDrugRequest> Get_Request_I_Made_IfExistsForUser(Guid reqId);
         Task<LzDrugRequest> Get_Request_I_Received_IfExistsForUser(Guid reqId);
    }
}
