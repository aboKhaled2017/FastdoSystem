using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fastdo.Core.ViewModels;
using Fastdo.API.Repositories;
using Fastdo.API.Services.Auth;
using Fastdo.Core;
using Fastdo.Core.Utilities;
using Fastdo.Core.Services.Auth;
using Microsoft.AspNetCore.Identity.UI.Services;
using Fastdo.Core.Services;

namespace Fastdo.API.Controllers
{
    [Route("api/phrdrgrequests")]
    [ApiController]
    [Authorize(Policy = "PharmacyPolicy")]
    public class LzDrgRequestsController : SharedAPIController
    {
        public LzDrgRequestsController(UserManager<AppUser> userManager, IEmailSender emailSender, IAccountService accountService, IMapper mapper, ITransactionService transactionService, IUnitOfWork unitOfWork) : base(userManager, emailSender, accountService, mapper, transactionService, unitOfWork)
        {
        }




        #region Get List Of LzDrug Requests
        [HttpGet("made", Name = "Get_AllRequests_I_Made")]
        public async Task<IActionResult> GetAllRequestsIMadeForUser([FromQuery]LzDrgReqResourceParameters _params)
        {
            var requests = await _unitOfWork.LzDrgRequestsRepository.Get_AllRequests_I_Made(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<Made_LzDrgRequest_MB, LzDrgReqResourceParameters>(
                requests, "Get_AllRequests_I_Made", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(requests);
        }
        [HttpGet("received",Name = "Get_AllRequests_I_Received")]
        public async Task<IActionResult> GetAllRequestsIReceivedForUser([FromQuery]LzDrgReqResourceParameters _params)
        {
            var requests = await  _unitOfWork.LzDrgRequestsRepository.Get_AllRequests_I_Received(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<Sent_LzDrgRequest_MB, LzDrgReqResourceParameters>(
                requests, "Get_AllRequests_I_Received", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(requests);
        }
        [HttpGet("received/ns", Name = "Get_AllRequests_I_Received_INS")]
        public async Task<IActionResult> GetNotSeenAllRequestesIReceivedForUser([FromQuery]LzDrgReqResourceParameters _params)
        {
            var requests = await  _unitOfWork.LzDrgRequestsRepository.Get_AllRequests_I_Received_INS(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<NotSeen_PhDrgRequest_MB, LzDrgReqResourceParameters>(
                requests, "Get_AllRequests_I_Received_INS", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(requests);
        }
        #endregion


        #region Get Single LzDrug Request
        [HttpGet("{id}", Name = "GetRequestById")]
        public async Task<IActionResult> GetLzDrgRequestByIdForUser(Guid id)
        {
            var req = await  _unitOfWork.LzDrgRequestsRepository.GetByIdAsync(id);
            if (req == null)
                return NotFound();
            return Ok(req);
        }
        #endregion

        #region (Add/handle/cancel) Single LzDrug Request
        [HttpPost("{drugId}",Name ="Add_LzDrug_Request_For_User")]
        public IActionResult PostNewRequestForUser(Guid drugId)
        {
            try
            {
                var req =  _unitOfWork.LzDrgRequestsRepository.AddForUser(drugId);
                if (req == null)
                    return BadRequest();
                var res =  _unitOfWork.LzDrgRequestsRepository.Save();
                if (!res)
                    return StatusCode(500, BasicUtility.MakeError("حدثت مشكلة اثناء معالجة طلبك ,من فضلك حاول مرة اخرى"));
                var reqObj = new
                {
                    req.Id,
                    req.LzDrugId,
                    req.PharmacyId,
                    req.Seen,
                    req.Status
                };
                return CreatedAtRoute(routeName: "GetRequestById", routeValues: new { id = req.Id }, reqObj);
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }
        [HttpPatch("received/{reqId}")]
        public async Task<IActionResult> PatchHandleRequestIReceivedForUser(Guid reqId,[FromBody] JsonPatchDocument<LzDrgRequest_ForUpdate_BM> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();
            var req = await  _unitOfWork.LzDrgRequestsRepository.Get_Request_I_Received_IfExistsForUser(reqId);
            if (req == null)
                return NotFound();
            var requestToPatch = _mapper.Map<LzDrgRequest_ForUpdate_BM>(req);
            patchDoc.ApplyTo(requestToPatch);
            //ad validation
            _mapper.Map(requestToPatch, req);
            var isSuccessfulluUpdated = await  _unitOfWork.LzDrgRequestsRepository.Patch_Update_Request_Sync(req);
            if (!isSuccessfulluUpdated)
                return StatusCode(500, BasicUtility.MakeError("لقد حدثت مشكلة اثناء معالجة طلبك , من فضلك حاول مرة اخرى"));
            return NoContent();
        }

        [HttpDelete("made/{reqId}")]
        public async Task<IActionResult> CancelRequestIMadeForUser([FromRoute]Guid reqId)
        {
            var req = await  _unitOfWork.LzDrgRequestsRepository.Get_Request_I_Made_IfExistsForUser(reqId);
            if (req == null)
                return NotFound();
             _unitOfWork.LzDrgRequestsRepository.Delete(req);
            if (!await  _unitOfWork.LzDrgRequestsRepository.SaveAsync())
                return StatusCode(500, BasicUtility.MakeError("حدثت مشكلة اثناء معالجة طلبك ,من فضلك حاول مرة اخرى"));
            return NoContent();

        }
        #endregion

        #region (handle/cancel) List Of LzDrug Requests

        [HttpDelete("Made/all")]
        public async Task<IActionResult> DeleteAllRequestsIMade()
        {
             _unitOfWork.LzDrgRequestsRepository.Delete_AllRequests_I_Made();
            if (!await  _unitOfWork.LzDrgRequestsRepository.SaveAsync())
                return StatusCode(500, BasicUtility.MakeError("لقد حدثت مشكلة اثناء معالجة طلبك , من فضلك حاول مرة اخرى"));
            return NoContent();
        }

        [HttpDelete("Made")]
        public async Task<IActionResult> DeleteAllRequestsIMade([FromBody] IEnumerable<Guid>Ids)
        {
            if (!await  _unitOfWork.LzDrgRequestsRepository.User_Made_These_Requests(Ids))
                return NotFound();
             _unitOfWork.LzDrgRequestsRepository.Delete_SomeRequests_I_Made(Ids);
            if (!await  _unitOfWork.LzDrgRequestsRepository.SaveAsync())
                return StatusCode(500, BasicUtility.MakeError("لقد حدثت مشكلة اثناء معالجة طلبك , من فضلك حاول مرة اخرى"));
            return NoContent();
        }

        [HttpPatch("received/({ids})")]
        public async Task<IActionResult> PatchHandleSomeRequestsIReceived(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid>Ids, 
            [FromBody] JsonPatchDocument<LzDrgRequest_ForUpdate_BM> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();
            if (!await  _unitOfWork.LzDrgRequestsRepository.User_Received_These_Requests(Ids))
                return NotFound();
            var reqs = await  _unitOfWork.LzDrgRequestsRepository.Get_Group_Of_Requests_I_Received(Ids);
            if (reqs.Count()==0)
                return NotFound();
            reqs.ToList().ForEach(req =>
            {
                var requestToPatch = _mapper.Map<LzDrgRequest_ForUpdate_BM>(req);
                patchDoc.ApplyTo(requestToPatch);
                //ad validation
                _mapper.Map(requestToPatch, req);

            });
             _unitOfWork.LzDrgRequestsRepository.Patch_Update_Group_Of_Requests_Sync(reqs);
            if (!await  _unitOfWork.LzDrgRequestsRepository.SaveAsync())
                return StatusCode(500, BasicUtility.MakeError("لقد حدثت مشكلة اثناء معالجة طلبك , من فضلك حاول مرة اخرى"));
            return NoContent();
        }
        #endregion

    }
}
