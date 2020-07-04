using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System_Back_End.Models;
using System_Back_End.Repositories;
using System_Back_End.Services.Auth;

namespace System_Back_End.Controllers
{
    [Route("api/phrdrgrequests")]
    [ApiController]
    [Authorize(Policy = "PharmacyPolicy")]
    public class LzDrgRequestsController : SharedAPIController
    {
        public ILzDrgRequestsRepository _lzDrgRequestsRepository { get; private set; }

        public LzDrgRequestsController(
            AccountService accountService, IMapper mapper,
            ILzDrgRequestsRepository lzDrgRequestsRepository,
            UserManager<AppUser> userManager)
            : base(accountService, mapper, userManager)
        {
            _lzDrgRequestsRepository = lzDrgRequestsRepository;
        }

        #region Get List Of LzDrug Requests
        [HttpGet("made", Name = "Get_AllRequests_I_Made")]
        public async Task<IActionResult> Get_AllRequests_I_Made([FromQuery]LzDrgReqResourceParameters _params)
        {
            var requests = await _lzDrgRequestsRepository.Get_AllRequests_I_Made(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<Made_LzDrgRequest_MB, LzDrgReqResourceParameters>(
                requests, "Get_AllRequests_I_Made", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(requests);
        }
        [HttpGet("received",Name = "Get_AllRequests_I_Received")]
        public async Task<IActionResult> Get_AllRequests_I_Received([FromQuery]LzDrgReqResourceParameters _params)
        {
            var requests = await _lzDrgRequestsRepository.Get_AllRequests_I_Received(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<Sent_LzDrgRequest_MB, LzDrgReqResourceParameters>(
                requests, "Get_AllRequests_I_Received", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(requests);
        }
        [HttpGet("received/ns", Name = "Get_AllRequests_I_Received_INS")]
        public async Task<IActionResult> Get_NotSeen_AllRequestes_I_Received([FromQuery]LzDrgReqResourceParameters _params)
        {
            var requests = await _lzDrgRequestsRepository.Get_AllRequests_I_Received_INS(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<NotSeen_PhDrgRequest_MB, LzDrgReqResourceParameters>(
                requests, "Get_AllRequests_I_Received_INS", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(requests);
        }
        #endregion


        #region Get Single LzDrug Request
        [HttpGet("{id}", Name = "GetRequestById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var req = await _lzDrgRequestsRepository.GetByIdAsync(id);
            if (req == null)
                return NotFound();
            return Ok(req);
        }
        #endregion

        #region (Add/handle/cancel) Single LzDrug Request
        [HttpPost("{drugId}")]
        public async Task<IActionResult> Post_NewRequest(Guid drugId)
        {
            var req = await _lzDrgRequestsRepository.AddForUserAsync(drugId);
            if (req == null)
                return NotFound();
            if (!await _lzDrgRequestsRepository.SaveAsync())
                return StatusCode(500, Functions.MakeError("حدثت مشكلة اثناء معالجة طلبك ,من فضلك حاول مرة اخرى"));
            return CreatedAtRoute(routeName: "GetRequestById", routeValues: new { id = req.Id }, req);
        }
        [HttpPatch("received/{reqId}")]
        public async Task<IActionResult> Patch_HandleRequest_I_Received(Guid reqId,[FromBody] JsonPatchDocument<LzDrgRequest_ForUpdate_BM> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();
            var req = await _lzDrgRequestsRepository.Get_Request_I_Received_IfExistsForUser(reqId);
            if (req == null)
                return NotFound();
            var requestToPatch = _mapper.Map<LzDrgRequest_ForUpdate_BM>(req);
            patchDoc.ApplyTo(requestToPatch);
            //ad validation
            _mapper.Map(requestToPatch, req);
            var isSuccessfulluUpdated = await _lzDrgRequestsRepository.Patch_UpdateSync(req);
            if (!isSuccessfulluUpdated)
                return StatusCode(500, Functions.MakeError("لقد حدثت مشكلة اثناء معالجة طلبك , من فضلك حاول مرة اخرى"));
            return NoContent();
        }

        [HttpDelete("made/{reqId}")]
        public async Task<IActionResult> Cancel_Request_I_Made(Guid reqId)
        {
            var req = await _lzDrgRequestsRepository.Get_Request_I_Made_IfExistsForUser(reqId);
            if (req == null)
                return NotFound();
            _lzDrgRequestsRepository.Delete(req);
            if (!await _lzDrgRequestsRepository.SaveAsync())
                return StatusCode(500, Functions.MakeError("حدثت مشكلة اثناء معالجة طلبك ,من فضلك حاول مرة اخرى"));
            return NoContent();

        }
        #endregion

        #region (handle/cancel) List Of LzDrug Requests

        [HttpPatch("/M{reqId}")]
        public async Task<IActionResult> Patchss(Guid reqId, [FromBody] JsonPatchDocument<LzDrgRequest_ForUpdate_BM> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();
            var req = await _lzDrgRequestsRepository.Get_Request_I_Received_IfExistsForUser(reqId);
            if (req == null)
                return NotFound();
            var requestToPatch = _mapper.Map<LzDrgRequest_ForUpdate_BM>(req);
            patchDoc.ApplyTo(requestToPatch);
            //ad validation
            _mapper.Map(requestToPatch, req);
            var isSuccessfulluUpdated = await _lzDrgRequestsRepository.Patch_UpdateSync(req);
            if (isSuccessfulluUpdated)
                return StatusCode(500, Functions.MakeError("لقد حدثت مشكلة اثناء معالجة طلبك , من فضلك حاول مرة اخرى"));
            return NoContent();
        }

        #endregion


    }
}
