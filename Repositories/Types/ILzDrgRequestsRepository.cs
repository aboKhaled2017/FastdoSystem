using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.backendsys.Models;
using Fastdo.Core;

namespace Fastdo.backendsys.Repositories
{
    public interface ILzDrgRequestsRepository :IRepository<LzDrugRequest>
    {
        Task<PagedList<Show_LzDrgsReq_ADM_Model>> GET_PageOf_LzDrgsRequests(LzDrgReqResourceParameters _params);
        Task<PagedList<Made_LzDrgRequest_MB>> Get_AllRequests_I_Made(LzDrgReqResourceParameters _params);
         Task<PagedList<Sent_LzDrgRequest_MB>> Get_AllRequests_I_Received(LzDrgReqResourceParameters _params);
         Task<PagedList<NotSeen_PhDrgRequest_MB>> Get_AllRequests_I_Received_INS(LzDrgReqResourceParameters _params);
         Task<IEnumerable<LzDrugRequest>> Get_Group_Of_Requests_I_Received(IEnumerable<Guid> reqIds);
         
         Task<LzDrugRequest> AddForUserAsync(Guid drugId);
         Task<bool> Patch_Update_Request_Sync(LzDrugRequest lzDrugRequest);
         void Patch_Update_Group_Of_Requests_Sync(IEnumerable<LzDrugRequest> lzDrugRequests);
         Task<bool> Make_RequestSeen(LzDrugRequest lzDrugRequest);
         void Delete(LzDrugRequest lzDrugRequest);
         void Delete_AllRequests_I_Made();
         void Delete_SomeRequests_I_Made(IEnumerable<Guid> Ids);
         Task<bool> User_Made_These_Requests(IEnumerable<Guid> Ids);
        Task<bool> User_Received_These_Requests(IEnumerable<Guid> Ids);
         Task<LzDrugRequest> Get_Request_I_Made_IfExistsForUser(Guid reqId);
         Task<LzDrugRequest> Get_Request_I_Received_IfExistsForUser(Guid reqId);
    }
}
