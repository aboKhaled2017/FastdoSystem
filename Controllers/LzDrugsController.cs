using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Fastdo.Core.ViewModels;
using Fastdo.API.Services.Auth;
using Fastdo.Core.Utilities;
using Fastdo.Core.Services;
using Fastdo.Core;
using Fastdo.Core.Services.Auth;

namespace Fastdo.API.Controllers
{
    [Route("api/lzdrugs")]
    [ApiController]
    [Authorize(Policy = "PharmacyPolicy")]
    public class LzDrugsController : SharedAPIController
    {
        private IUrlHelper _urlHelper { get; }
        public LzDrugsController(UserManager<AppUser> userManager, IEmailSender emailSender, 
            IAccountService accountService, IMapper mapper, 
            ITransactionService transactionService, Core.IUnitOfWork unitOfWork,  IUrlHelper urlHelper) : base(userManager, emailSender, accountService, mapper, transactionService, unitOfWork)
        {
            _urlHelper = urlHelper;
        }


        #region get

        [HttpGet(Name ="GetAllLzDrugsForCurrentUser")]
        public async Task<IActionResult> GetAllDrugs([FromQuery]LzDrgResourceParameters _params)
        {
            var allDrugsData =await  _unitOfWork.LzDrugRepository.GetAll_BM(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<LzDrugModel_BM, LzDrgResourceParameters>(
                allDrugsData, "GetAllLzDrugsForCurrentUser", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader,paginationMetaData);
            return Ok(allDrugsData);
        }

        // GET: api/LzDrugs/5
        [HttpGet("{id}", Name = "GetDrugById")]
        public async Task<IActionResult> GetDrugById(Guid id)
        {
            var drug = await _unitOfWork.LzDrugRepository.Get_BM_ByIdAsync(id);
            if (drug == null)
                return NotFound();
            return Ok(drug);
        }
        #endregion

        #region post
        // POST: api/LzDrugs
        [HttpPost]
        public async Task<IActionResult> PostDrug([FromBody] AddLzDrugModel drugModel)
        {
            if (!ModelState.IsValid)
                return new Microsoft.AspNetCore.Mvc.UnprocessableEntityObjectResult(ModelState);
            var drug = _mapper.Map<LzDrug>(drugModel);
            _unitOfWork.LzDrugRepository.Add(drug);
            if (! _unitOfWork.Save())
                return StatusCode(500, BasicUtility.MakeError("حدثت مشكلة اثناء معالجة طلبك"));
            return CreatedAtRoute(
                routeName: "GetDrugById",
                routeValues: new { id = drug.Id}, 
                _mapper.Map<LzDrugModel_BM>(drug));
        }

        #endregion

        #region put
        // PUT: api/LzDrugs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDrug(Guid id, [FromBody] UpdateLzDrugModel drugModel)
        {
            if (!ModelState.IsValid)
                return new Core.UnprocessableEntityObjectResult(ModelState);
            if (id != drugModel.Id)
                return BadRequest();
            if (!await _unitOfWork.LzDrugRepository.IsUserHas(id))
                return NotFound();
            var drug = _mapper.Map<LzDrug>(drugModel);
            _unitOfWork.LzDrugRepository.Update(drug);
            if (!await _unitOfWork.LzDrugRepository.SaveAsync())
                return StatusCode(500, BasicUtility.MakeError("حدثت مشكلة اثناء معالجة طلبك"));
            return NoContent();
        }

        #endregion

        #region delete
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDrug(Guid id)
        {
            if (!await _unitOfWork.LzDrugRepository.IsUserHas(id))
                return NotFound();
            var drugToDelete =await _unitOfWork.LzDrugRepository.GetByIdAsync(id);
            _unitOfWork.LzDrugRepository.Remove(drugToDelete);
            if (!await _unitOfWork.LzDrugRepository.SaveAsync())
                return StatusCode(500, BasicUtility.MakeError("حدثت مشكلة اثناء معالجة طلبك"));
            return NoContent();
        }
        #endregion
    }
}
