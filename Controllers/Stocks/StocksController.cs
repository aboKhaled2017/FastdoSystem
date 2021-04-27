using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fastdo.Core.ViewModels.Stocks.Models;
using Fastdo.Core.ViewModels;
using Fastdo.API.Repositories;
using Fastdo.API.Services;
using Fastdo.API.Services.Auth;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Fastdo.API.Controllers;
using Fastdo.Core.Services;
using Fastdo.Core.Utilities;
using Fastdo.Core.Services.Auth;

namespace Fastdo.Core.ViewModels.Stocks
{
    [Route("api/stk")]
    [ApiController]
    [Authorize(Policy = "StockPolicy")]
    public class StocksController : SharedAPIController
    {
        public StocksController(UserManager<AppUser> userManager, IEmailSender emailSender,
            IAccountService accountService, IMapper mapper, ITransactionService transactionService, 
            IUnitOfWork unitOfWork,
                StkDrugsReportFromExcelService reportService, StockUserServices stockUsersService) : base(userManager, emailSender, accountService, mapper, transactionService, unitOfWork)
        {
            _stkDrugsReportFromExcelService = reportService;
            _stockUserServices = stockUsersService;
        }


        #region constructors and properties
        public StkDrugsReportFromExcelService _stkDrugsReportFromExcelService { get; private set; }
        public StockUserServices _stockUserServices { get; private set; }
        
        #endregion

        #region override methods from parent class
        [ApiExplorerSettings(IgnoreApi = true)]
        public override string Create_BMs_ResourceUri(IResourceParameters _params, ResourceUriType resourceUriType, string routeName)
        {
            var _cardParams = _params as LzDrgResourceParameters;
            switch (resourceUriType)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(routeName,
                    new LzDrgResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber - 1,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S
                    }); ;
                case ResourceUriType.NextPage:
                    return Url.Link(routeName,
                    new LzDrgResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber + 1,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S
                    });
                default:
                    return Url.Link(routeName,
                    new LzDrgResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S
                    });
            }            
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string Create_StkDrusPackageReqs_ResourceUri(ResourceParameters _params, ResourceUriType resourceUriType, string routeName)
        {
            var _cardParams = _params as StkDrugsPackageReqResourceParmaters;
            switch (resourceUriType)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(routeName,
                    new StkDrugsPackageReqResourceParmaters
                    {
                        PageNumber = _cardParams.PageNumber - 1,
                        PageSize = _cardParams.PageSize,
                        Status=_cardParams.Status
                    }); ;
                case ResourceUriType.NextPage:
                    return Url.Link(routeName,
                    new StkDrugsPackageReqResourceParmaters
                    {
                        PageNumber = _cardParams.PageNumber + 1,
                        PageSize = _cardParams.PageSize,
                        Status = _cardParams.Status
                    });
                default:
                    return Url.Link(routeName,
                    new StkDrugsPackageReqResourceParmaters
                    {
                        PageNumber = _cardParams.PageNumber,
                        PageSize = _cardParams.PageSize,
                        Status = _cardParams.Status
                    });
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string CreateResourceUriForPhamaReques(ResourceParameters _params, ResourceUriType resourceUriType, string routeName)
        {
            var _requestsParams = _params as PharmaReqsResourceParameters;
            switch (resourceUriType)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(routeName,
                    new PharmaReqsResourceParameters
                    {
                        PageNumber = _requestsParams.PageNumber - 1,
                        PageSize = _requestsParams.PageSize,
                        S = _requestsParams.S,
                        PharmaClass = _requestsParams.PharmaClass,
                        Status = _requestsParams.Status
                    }); ;
                case ResourceUriType.NextPage:
                    return Url.Link(routeName,
                    new PharmaReqsResourceParameters
                    {
                        PageNumber = _requestsParams.PageNumber + 1,
                        PageSize = _requestsParams.PageSize,
                        S = _requestsParams.S,
                        PharmaClass = _requestsParams.PharmaClass,
                        Status = _requestsParams.Status
                    });
                default:
                    return Url.Link(routeName,
                    new PharmaReqsResourceParameters
                    {
                        PageNumber = _requestsParams.PageNumber,
                        PageSize = _requestsParams.PageSize,
                        S = _requestsParams.S,
                        PharmaClass = _requestsParams.PharmaClass,
                        Status = _requestsParams.Status
                    });
            }
        }
       
        #endregion

        #region update the stock report
        [HttpPut("prods")]
        public async Task<IActionResult> UpdateDrugsReporst([FromForm] StockDrugsReporstModel model)
        {
            //var id = _userManager.GetUserId(User);
            var id = _userManager.GetUserId(User);
            if (!_stockUserServices.IsStockHasClass(model.ForClassId,User))
                return BadRequest(BasicUtility.MakeError(nameof(model.ForClassId),"هذا التصنيف غير موجود"));
            var currentDrugs =await _unitOfWork.StkDrugsRepository.GetDiscountsForEachStockDrug(id);
            var response = _stkDrugsReportFromExcelService.ProcessFileAndGetReport(id, currentDrugs, model);
            if (!response.Status)
                return BadRequest(BasicUtility.MakeError(response.ErrorMess));
            try
            {
                _transactionService.Begin();
                await  _unitOfWork.StkDrugsRepository.AddListOfDrugs(response.StkDrugsReport.ToList(), currentDrugs, id);
                _transactionService.CommitChanges().End();
                    return NoContent();
            }
            catch(Exception e)
            {
                _transactionService.RollBackChanges().End();
                return BadRequest(BasicUtility.MakeError(e.Message));
            }         
        }

        #endregion

        #region GET
        [HttpGet("prods",Name ="GetStockDrugsOfReport")]
        public async Task<IActionResult> GetStockDrugsOfReport([FromQuery]LzDrgResourceParameters _params)
        {
            var data = await  _unitOfWork.StkDrugsRepository.GetAllStockDrugsOfReport(_userManager.GetUserId(User), _params);
            var paginationMetaData = new PaginationMetaDataGenerator<StkShowDrugModel, LzDrgResourceParameters>(
                data, "GetStockDrugsOfReport", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(data);
        }

        [HttpGet("joinRequests", Name = "GetJoinReqsPharmacies")]
        public async Task<IActionResult> GetJoinedRequestsPharmas([FromQuery]PharmaReqsResourceParameters _params)
        {
            var requests = await _unitOfWork.StockRepository.GetJoinRequestsPharmas(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<ShowJoinRequestToStkModel, PharmaReqsResourceParameters>(
                requests, "GetJoinReqsPharmacies", _params, CreateResourceUriForPhamaReques
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(requests);
        }

        [HttpGet("joinedPharmas", Name = "GetJoinedPharmacies")]
        public async Task<IActionResult> GetJoinedPharmas([FromQuery]PharmaReqsResourceParameters _params)
        {
            var requests = await _unitOfWork.StockRepository.GetJoinedPharmas(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<ShowJoinedPharmaToStkModel, PharmaReqsResourceParameters>(
                requests, "GetJoinedPharmacies", _params, CreateResourceUriForPhamaReques
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(requests);
        }
        
        [HttpGet("drugsReqs",Name ="GetStkDrugsPackageRequests")]
        public async Task<IActionResult> GetStockRequestsOfDrugsPackage([FromQuery] StkDrugsPackageReqResourceParmaters _params)
        {
            var requests = await _unitOfWork.StockRepository.GetStkDrugsPackageRequests(_params);
            var paginationMetaData = 
                new PaginationMetaDataGenerator<StkDrugsPackageReqModel, StkDrugsPackageReqResourceParmaters>(
                requests, "GetStkDrugsPackageRequests", _params, Create_StkDrusPackageReqs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(requests);
        }

        #endregion

        #region Patch
        [HttpPatch("pharmaReqs/{pharmaId}")]
        public async Task<IActionResult> HandlePharmaRequest(string pharmaId, [FromBody] JsonPatchDocument<HandlePharmaRequestModel> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();
            if (!await _unitOfWork.StockRepository.HandlePharmacyRequest(pharmaId, request => {

                var model = _mapper.Map<HandlePharmaRequestModel>(request);
                patchDoc.ApplyTo(model);
                //ad validation
                _mapper.Map(model, request);
            }))
                return NotFound();
            return NoContent();
        }

        [HttpPatch("drugsReqs/{packageId}")]
        public async Task<IActionResult> HandlePharmaRequest([FromRoute]Guid packageId, [FromBody] JsonPatchDocument<dynamic> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();
            dynamic _error = null;
            await _unitOfWork.StockRepository.HandleStkDrugsPackageRequest_ForStock(packageId,
                request =>
                {
                    patchDoc.ApplyTo(request);
                },
                error =>
                {
                    _error = error;
                });
            if (_error != null)
                return BadRequest(_error);
            return NoContent();
        }
        #endregion

        #region Update

        [HttpPut("joinedPharmas")]
        public async Task<IActionResult> updateJoinedPharmaClass(AssignAnotherClassForPharmacyModel model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            if (
                model.getNewClassId==null ||
                model.getNewClassId==Guid.Empty |
                model.getOldClassId == null ||
                model.getOldClassId == Guid.Empty)
                return BadRequest();
            if (!_stockUserServices.IsStockHasClass(model.getNewClassId, User))
                return BadRequest(BasicUtility.MakeError(nameof(model.NewClassId), "هذا التصنيف غير موجود"));
            if (!_stockUserServices.IsStockHasClass(model.getOldClassId, User))
                return BadRequest(BasicUtility.MakeError(nameof(model.OldClassId), "هذا التصنيف غير موجود"));
            dynamic error = null;
            await _unitOfWork.StockRepository.AssignAnotherClassForPharmacy(model,err=> {
                error = err;
            });
            if(error!=null)
                return BadRequest(error);
            return NoContent();
        }
        #endregion

        #region delete
        // DELETE: api/ApiWithActions/5
        [HttpDelete("prods/{id}")]
        public async Task<IActionResult> DeleteDrug(Guid id)
        {
            if (!await  _unitOfWork.StkDrugsRepository.IsUserHas(id))
                return NotFound();
            var drugToDelete = await  _unitOfWork.StkDrugsRepository.GetByIdAsync(id);
             _unitOfWork.StkDrugsRepository.Remove(drugToDelete);
            if (!await  _unitOfWork.StkDrugsRepository.SaveAsync())
                return StatusCode(500, BasicUtility.MakeError("حدثت مشكلة اثناء معالجة طلبك"));
            return NoContent();
        }

        
        [HttpDelete("prods")]
        public IActionResult DeleteAllDrugs()
        {
            _transactionService.Begin();
             _unitOfWork.StkDrugsRepository.DeleteAll();
            _transactionService.CommitChanges();
            /*if (!await  _unitOfWork.StkDrugsRepository.SaveAsync())
                return StatusCode(500, BasicUtility.MakeError("حدثت مشكلة اثناء معالجة طلبك"));*/
            return NoContent();
        }

        #endregion

        #region Stock classes for pharmas [rename/delete]
        [HttpPost("phclasses/{NewClass}")]
        public async Task<IActionResult> AddStockClassForPharma([Required(ErrorMessage ="ادخل قيمة")]string NewClass)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            if (string.IsNullOrWhiteSpace(NewClass))
                return BadRequest();
            if (_stockUserServices.IsStockHasClassName(NewClass,User))
                return BadRequest(BasicUtility.MakeError(nameof(NewClass),"هذا التصنيف موجود بالفعل"));
            await _unitOfWork.StockRepository.AddNewPharmaClass(NewClass);
            return Ok(await _accountService.GetSigningInResponseModelForStock(await _userManager.FindByIdAsync(_userManager.GetUserId(User))));

        }
        [HttpDelete("phclasses")]
        public async Task<IActionResult> DeleteStockClassForPharma(DeleteStockClassForPharmaModel model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            if (model.getDeletedClassId==null || model.getDeletedClassId==Guid.Empty)
                return BadRequest();
            if (_stockUserServices.IsStockHasSinglePharmaClasses(User))
                return BadRequest(BasicUtility.MakeError("لا يمكن حذف التصنيف الافتراضى ,يمكنك فقط اعادة تسميته"));
            if (!_stockUserServices.IsStockHasClass(model.getDeletedClassId, User))
                return BadRequest(BasicUtility.MakeError(nameof(model.DeletedClassId), "هذا التصنيف غير موجود"));
            var classes = _stockUserServices.GetStockClassesForPharmas(User);
            if(classes.Any(c=>c.Id==model.getDeletedClassId && c.Count > 0))
            {
                if(!_stockUserServices.IsStockHasClass(model.getReplaceClassId,User))
                    return BadRequest(BasicUtility.MakeError(nameof(model.ReplaceClassId), "هذا التصنيف غير موجود"));
            }
            dynamic _error = null;
            _transactionService.Begin();
            await _unitOfWork.StockRepository.RemovePharmaClass(model,error=> {
                _error = error;
                _transactionService.RollBackChanges().End();
            });
            if (_error != null)
                return NotFound(_error);
            _transactionService.CommitChanges().End();
            return Ok(await _accountService.GetSigningInResponseModelForStock(await _userManager.FindByIdAsync(_userManager.GetUserId(User))));

        }
      
        [HttpPut("phclasses")]
        public async Task<IActionResult> RenameStockClassForPharma(UpdateStockClassForPharmaModel model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            if (string.IsNullOrWhiteSpace(model.NewClass)|| string.IsNullOrWhiteSpace(model.OldClass))
                return BadRequest();
            if (!_stockUserServices.IsStockHasClassName(model.OldClass,User))
                return BadRequest(BasicUtility.MakeError(nameof(model.OldClass), "هذا التصنيف غير موجود"));
            if (_stockUserServices.IsStockHasClassName(model.NewClass,User))
                return BadRequest(BasicUtility.MakeError(nameof(model.NewClass), "هذا التصنيف موجود بالفعل"));
            await _unitOfWork.StockRepository.RenamePharmaClass(model);
            return Ok(await _accountService.GetSigningInResponseModelForStock(await _userManager.FindByIdAsync(_userManager.GetUserId(User))));

        }       
        #endregion

        #region private methods 


        #endregion
    }
}