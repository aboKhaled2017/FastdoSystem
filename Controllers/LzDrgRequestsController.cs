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
        [HttpGet("made", Name = "GetMadeLzDrugRequests")]
        public async Task<IActionResult> RequestsMadeByMe([FromQuery]LzDrgReqResourceParameters _params)
        {
            var requests = await _lzDrgRequestsRepository.GetMadeRequestsByUser(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<Made_LzDrgRequest_MB, LzDrgReqResourceParameters>(
                requests, "GetMadeLzDrugRequests", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(requests);
        }
        [HttpGet(Name = "GetSentLzDrugRequests")]
        public async Task<IActionResult> RequestsSentToMe([FromQuery]LzDrgReqResourceParameters _params)
        {
            var requests = await _lzDrgRequestsRepository.GetSentRequestsForUser(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<Sent_LzDrgRequest_MB, LzDrgReqResourceParameters>(
                requests, "GetSentLzDrugRequests", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(requests);
        }
        [HttpGet("notseen", Name = "GetNotSeenLzDrugRequests")]
        public async Task<IActionResult> GetNotSeenRequestes([FromQuery]LzDrgReqResourceParameters _params)
        {
            var requests = await _lzDrgRequestsRepository.GetNotSeenRequestsByUser(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<NotSeen_PhDrgRequest_MB, LzDrgReqResourceParameters>(
                requests, "GetNotSeenLzDrugRequests", _params, Create_BMs_ResourceUri
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
        public async Task<IActionResult> Post(Guid drugId)
        {
            var req = await _lzDrgRequestsRepository.AddForUserAsync(drugId);
            if (req == null)
                return NotFound();
            if (!await _lzDrgRequestsRepository.SaveAsync())
                return StatusCode(500, Functions.MakeError("حدثت مشكلة اثناء معالجة طلبك ,من فضلك حاول مرة اخرى"));
            return CreatedAtRoute(routeName: "GetRequestById", routeValues: new { id = req.Id }, req);
        }
        [HttpPatch("{reqId}")]
        public async Task<IActionResult> Patch(Guid reqId,[FromBody] JsonPatchDocument<LzDrgRequest_ForUpdate_BM> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();
            var req = await _lzDrgRequestsRepository.GetSentRquestIfExistsForUser(reqId);
            if (req == null)
                return NotFound();
            var requestToPatch = _mapper.Map<LzDrgRequest_ForUpdate_BM>(req);
            patchDoc.ApplyTo(requestToPatch);
            //ad validation
            _mapper.Map(requestToPatch, req);
            var isSuccessfulluUpdated = await _lzDrgRequestsRepository.PatchUpdateSync(req);
            if (isSuccessfulluUpdated)
                return StatusCode(500, Functions.MakeError("لقد حدثت مشكلة اثناء معالجة طلبك , من فضلك حاول مرة اخرى"));
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{reqId}")]
        public async Task<IActionResult> Cancel(Guid reqId)
        {
            var req =await _lzDrgRequestsRepository.GetMadeRquestIfExistsForUser(reqId);
            if (req == null)
                return NotFound();
            _lzDrgRequestsRepository.Delete(req);
            if(! await _lzDrgRequestsRepository.SaveAsync())
                return StatusCode(500, Functions.MakeError("حدثت مشكلة اثناء معالجة طلبك ,من فضلك حاول مرة اخرى"));
            return NoContent();

        }
        #endregion

        #region (handle/cancel) List Of LzDrug Requests

        [HttpPatch("{reqId}")]
        public async Task<IActionResult> Patchss(Guid reqId, [FromBody] JsonPatchDocument<LzDrgRequest_ForUpdate_BM> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();
            var req = await _lzDrgRequestsRepository.GetSentRquestIfExistsForUser(reqId);
            if (req == null)
                return NotFound();
            var requestToPatch = _mapper.Map<LzDrgRequest_ForUpdate_BM>(req);
            patchDoc.ApplyTo(requestToPatch);
            //ad validation
            _mapper.Map(requestToPatch, req);
            var isSuccessfulluUpdated = await _lzDrgRequestsRepository.PatchUpdateSync(req);
            if (isSuccessfulluUpdated)
                return StatusCode(500, Functions.MakeError("لقد حدثت مشكلة اثناء معالجة طلبك , من فضلك حاول مرة اخرى"));
            return NoContent();
        }

        #endregion


    }
}
