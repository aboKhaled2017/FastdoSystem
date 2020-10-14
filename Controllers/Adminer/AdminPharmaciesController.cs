using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fastdo.backendsys.Models;
using Fastdo.backendsys.Repositories;
using Fastdo.backendsys.Services;
using Fastdo.backendsys.Services.Auth;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Fastdo.backendsys.Controllers.Adminer
{
    [Route("api/admins/pharmacies", Name = "AdminPharmacies")]
    [ApiController]
    [Authorize(Policy = "ControlOnPharmaciesPagePolicy")]
    public class AdminPharmaciesController : MainAdminController
    {
        #region constructor and properties
        private readonly IPharmacyRepository _pharmacyRepository;
        public AdminPharmaciesController(UserManager<AppUser> userManager, IEmailSender emailSender,
            AccountService accountService, IMapper mapper, TransactionService transactionService,
            IPharmacyRepository pharmacyRepository) : base(userManager, emailSender, accountService, mapper, transactionService)
        {
            _pharmacyRepository = pharmacyRepository;
        }
        #endregion

        #region override methods from parent class
        [ApiExplorerSettings(IgnoreApi = true)]
        public override string Create_BMs_ResourceUri(ResourceParameters _params, ResourceUriType resourceUriType, string routeName)
        {
            var _cardParams = _params as PharmaciesResourceParameters;
            switch (resourceUriType)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(routeName,
                    new PharmaciesResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber - 1,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S,
                        Status = _cardParams.Status,
                        OrderBy = _cardParams.OrderBy
                    });
                case ResourceUriType.NextPage:
                    return Url.Link(routeName,
                    new PharmaciesResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber + 1,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S,
                        Status = _cardParams.Status,
                        OrderBy = _cardParams.OrderBy
                    });
                default:
                    return Url.Link(routeName,
                    new PharmaciesResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S,
                        Status=_cardParams.Status,
                        OrderBy = _cardParams.OrderBy
                    });
            }
        }


        #endregion

        #region get
        [HttpGet("{id}")]
        public async Task<ActionResult<Get_PageOf_Pharmacies_ADMModel>> GetPharmacyByIdForAdmin([FromRoute]string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();
            var pharm =await _pharmacyRepository.Get_PharmacyModel_ADM(id);
            if (pharm == null)
                return NotFound();
            return Ok(pharm);
        }
        [HttpGet(Name ="Get_PageOfPharmacies_ADM")]
        public async Task<IActionResult> GetPageOfPharmaciesForAdmin([FromQuery]PharmaciesResourceParameters _params)
        {
            var pharms = await _pharmacyRepository.Get_PageOf_PharmacyModels_ADM(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<Get_PageOf_Pharmacies_ADMModel, PharmaciesResourceParameters>(
                pharms, "Get_PageOfPharmacies_ADM", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(pharms);
        }
        #endregion

        #region delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePharmacyForAdmin([FromRoute]string id)
        {
            var pharm =await _pharmacyRepository.GetByIdAsync(id);
            if (pharm == null)
                return NotFound();
              _pharmacyRepository.Remove(pharm);
            if (!await _pharmacyRepository.SaveAsync())
                return StatusCode(500, Functions.MakeError("حدثت مشكلة اثناء معالجة طلبك ,من فضلك حاول مرة اخرى"));
            return NoContent();
        }
        #endregion

        #region Patch
        [HttpPatch("{id}")]
        public async Task<IActionResult> HandlePharmacyRequestFromAdmin([FromRoute]string id, [FromBody] JsonPatchDocument<Pharmacy_Update_ADM_Model> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();
            var pharm = await _pharmacyRepository.GetByIdAsync(id);
            if (pharm == null)
                return NotFound();
            var requestToPatch = _mapper.Map<Pharmacy_Update_ADM_Model>(pharm);
            patchDoc.ApplyTo(requestToPatch);
            //ad validation
            _mapper.Map(requestToPatch, pharm);
            var isSuccessfulluUpdated = await _pharmacyRepository.Patch_Apdate_ByAdmin(pharm);
            if (!isSuccessfulluUpdated)
                return StatusCode(500, Functions.MakeError("لقد حدثت مشكلة اثناء معالجة طلبك , من فضلك حاول مرة اخرى"));
            return NoContent();
        }
        #endregion
    }
}