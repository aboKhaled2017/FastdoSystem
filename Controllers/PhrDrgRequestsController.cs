using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class PhrDrgRequestsController : SharedAPIController
    {
        public PhrDrgRequestsRepository _phrDrgRequestsRepository { get; private set; }

        public PhrDrgRequestsController(
            AccountService accountService, IMapper mapper,
            PhrDrgRequestsRepository phrDrgRequestsRepository,
            UserManager<AppUser> userManager) 
            : base(accountService, mapper, userManager)
        {
            _phrDrgRequestsRepository = phrDrgRequestsRepository;
        }

        // GET: api/PhrDrgRequests
        [HttpGet("made",Name ="GetMadeLzDrugRequests")]
        public async Task<IActionResult> RequestsMadeByMe([FromQuery]LzDrReqResourceParameters _params)
        {
            var requests =await _phrDrgRequestsRepository.GetMadeRequestsByUser(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<Made_PhDrgRequest_MB, LzDrReqResourceParameters>(
                requests, "GetMadeLzDrugRequests", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(requests);
        }
        [HttpGet(Name = "GetSentLzDrugRequests")]
        public async Task<IActionResult> RequestsSentToMe([FromQuery]LzDrReqResourceParameters _params)
        {
            var requests = await _phrDrgRequestsRepository.GetSentRequestsForUser(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<Sent_PhDrgRequest_MB, LzDrReqResourceParameters>(
                requests, "GetSentLzDrugRequests", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(requests);
        }
        [HttpGet("notseen",Name = "GetNotSeenLzDrugRequests")]
        public async Task<IActionResult> GetNotSeenRequestes([FromQuery]LzDrReqResourceParameters _params)
        {
            var requests = await _phrDrgRequestsRepository.GetNotSeenRequestsByUser(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<NotSeen_PhDrgRequest_MB, LzDrReqResourceParameters>(
                requests, "GetNotSeenLzDrugRequests", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(requests);
        }
        // GET: api/PhrDrgRequests/5
        [HttpGet("{id}", Name = "GetRequestById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var req =await _phrDrgRequestsRepository.GetByIdAsync(id);
            if (req == null)
                return NotFound();
            return Ok(req);
        }

        // POST: api/PhrDrgRequests
        [HttpPost("{drugId}")]
        public async Task<IActionResult> Post(Guid drugId)
        {
            var req =await _phrDrgRequestsRepository.AddForUserAsync(drugId);
            if (req == null)
                return BadRequest();
            if (!await _phrDrgRequestsRepository.SaveAsync())
                return StatusCode(500, Functions.MakeError("حدثت مشكلة اثناء معالجة طلبك ,من فضلك حاول مرة اخرى"));
            return CreatedAtRoute(routeName: "GetRequestById", routeValues: new { id = req.Id }, req);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{reqId}")]
        public async Task<IActionResult> Delete(Guid reqId)
        {
            var req =await _phrDrgRequestsRepository.GetRquestIfExistsForUser(reqId);
            if (req == null)
                return BadRequest();
            _phrDrgRequestsRepository.Delete(req);
            if(! await _phrDrgRequestsRepository.SaveAsync())
                return StatusCode(500, Functions.MakeError("حدثت مشكلة اثناء معالجة طلبك ,من فضلك حاول مرة اخرى"));
            return NoContent();

        }
        [HttpPatch("seen/{reqId}")]
        public async Task<IActionResult> MakeRequestSeen(Guid reqId)
        {
            var req = await _phrDrgRequestsRepository.GetSentRquestIfExistsForUser(reqId);
            if (req == null)
                return BadRequest();
            var res=await _phrDrgRequestsRepository.MakeRequestSeen(req);
            if (!res)
                return StatusCode(500, Functions.MakeError("حدثت مشكلة اثناء معالجة طلبك ,من فضلك حاول مرة اخرى"));
            return NoContent();

        }

    }
}
