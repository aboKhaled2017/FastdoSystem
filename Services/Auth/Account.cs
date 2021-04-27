using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text;
using Fastdo.Core.ViewModels;
using AutoMapper;
using Fastdo.API.Repositories;
using System.Security.Claims;
using Newtonsoft.Json;
using Fastdo.Core;
using Fastdo.Core.Services.Auth;
using Fastdo.Core.Services;
using Fastdo.Core.Utilities;
using Fastdo.Core.Enums;
using Fastdo.API.Services.Auth;

namespace Fastdo.API.Services.Auth
{
    
    public class AccountService: IAccountService
    {
        #region constructor and properties
        private readonly JWThandlerService _jWThandlerService;
        private readonly IEmailSender _emailSender;

        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ITransactionService _transactionService { get; }
        private HttpContext _httpContext { get; set; }
        private IUrlHelper _Url { get; set; }
        private readonly IConfigurationSection _JWT = RequestStaticServices.GetConfiguration().GetSection("JWT");

        public AccountService(
            IEmailSender emailSender,
            JWThandlerService jWThandlerService,
            UserManager<AppUser> userManager,
            ITransactionService transactionService,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _jWThandlerService = jWThandlerService;
            _userManager = userManager;
            _emailSender = emailSender;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _transactionService = transactionService;
        }
        #endregion

        public void SetCurrentContext(HttpContext httpContext, IUrlHelper url)
        {
            _httpContext = httpContext;
            _Url = url;
        }
        public async Task<ISigningResponseModel> GetSigningInResponseModelForAdministrator(AppUser user, Admin admin, string adminType)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var _user = _mapper.MergeInto<AdministratorClientResponseModel>(user, admin);
            _user.Prevligs = claims.FirstOrDefault(c => c.Type == Variables.AdminClaimsTypes.Priviligs).Value;
            return new SigningAdministratorClientInResponseModel
            {
                user = _user,
                accessToken = _jWThandlerService.CreateAccessToken_ForAdministartor(user, admin.Name, claims)
            };

        }
        public ISigningResponseModel GetSigningInResponseModelForPharmacy(AppUser user, Pharmacy pharmacy)
        {
            var userResponse = _mapper.MergeInto<PharmacyClientResponseModel>(user, pharmacy);
            return new SigningPharmacyClientInResponseModel
                {
                    user = userResponse,
                    accessToken = _jWThandlerService.CreateAccessToken(
                        user,
                        Variables.pharmacier,
                        pharmacy.Name)
                };   
                      
        }
        public ISigningResponseModel GetSigningInResponseModelForStock(AppUser user, Stock stock)
        {

            var responseUser = _mapper.MergeInto<StockClientResponseModel>(user, stock);
            var classes =_unitOfWork.StockRepository.GetStockClassesOfJoinedPharmas(user.Id).Result;
            responseUser.PharmasClasses =classes;
            return new SigningStockClientInResponseModel
            {
                user = responseUser,
                accessToken = _jWThandlerService.CreateAccessToken(
                    user, Variables.stocker,
                    stock.Name,
                    claims=> {
                        claims.Add(new Claim(Variables.StockUserClaimsTypes.PharmasClasses, JsonConvert.SerializeObject(classes))); 
                    })
            };
        }

        
        public async Task<ISigningResponseModel> GetSigningInResponseModelForCurrentUser(AppUser user)
        {
            var userType = BasicUtility.CurrentUserType();
            if (userType == UserType.pharmacier)
            {
                var pharmacy = await _unitOfWork.PharmacyRepository.GetByIdAsync(user.Id);
                return GetSigningInResponseModelForPharmacy(user, pharmacy);
            }
            else
            {
                var stock = await _unitOfWork.StockRepository.GetByIdAsync(user.Id);
                return GetSigningInResponseModelForStock(user, stock);
            }
        }
        public async Task<ISigningResponseModel> GetSigningInResponseModelForAdministrator(AppUser user,string adminType)
        {
            var admin = await _unitOfWork.AdminRepository.GetByIdAsync(user.Id);
            return await GetSigningInResponseModelForAdministrator(user, admin, adminType);
        }
        public async Task<ISigningResponseModel> GetSigningInResponseModelForPharmacy(AppUser user)
        {
            var pharmacy = await _unitOfWork.PharmacyRepository.GetByIdAsync(user.Id);
            return GetSigningInResponseModelForPharmacy(user, pharmacy);
        }

        public async Task<ISigningResponseModel> GetSigningInResponseModelForStock(AppUser user)
        {
            var stock = await _unitOfWork.StockRepository.GetByIdAsync(user.Id);
            return GetSigningInResponseModelForStock(user, stock);
        }

        public async Task<ISigningResponseModel> SignUpPharmacyAsync(PharmacyClientRegisterModel model, IExecuterDelayer executerDelayer)
        {
            //the email is already checked at validation if it was existed before for any user
            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PersPhone,
                confirmCode = BasicUtility.GenerateConfirmationTokenCode()
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return null;
            result = await _userManager.AddToRoleAsync(user, Variables.pharmacier);
            if (!result.Succeeded)
                return null;
            //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var callbackUrl = _Url.EmailConfirmationLink(user.Id.ToString(), code, _httpContext.Request.Scheme);
            executerDelayer.OnExecuting = async () =>
            {
                await _emailSender.SendEmailAsync(
                    user.Email,
                    "كود تأكيد البريد الالكترونى", $"كود التأكيد الخاص بك هو: {user.confirmCode}");
            };
            //ActionOnResult(false, result, user);
            var pharmacy = _mapper.Map<Pharmacy>(model);
            pharmacy.Id = user.Id;
            return GetSigningInResponseModelForPharmacy(user, pharmacy);
        }
        public async Task<ISigningResponseModel> SignUpStockAsync(
            StockClientRegisterModel model,
            Action<string> ExecuteOnError,
            Action<Stock,Action> AddStockModelToRepo,
            Action OnFinsh)
        {
            //the email is already checked at validation if it was existed before for any user
            var user = new AppUser {
                UserName = model.Email,
                Email = model.Email, 
                PhoneNumber = model.PersPhone,
                confirmCode=BasicUtility.GenerateConfirmationTokenCode() };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                ExecuteOnError.Invoke("لايمكن اضافة هذا المستخدم");return null;
            }
                result = await _userManager.AddToRoleAsync(user, Variables.stocker);
            if (!result.Succeeded)
            {
                ExecuteOnError.Invoke("لايمكن اضافة هذا المستخدم الى roles"); return null;
            }
            //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var callbackUrl = _Url.EmailConfirmationLink(user.Id.ToString(), code, _httpContext.Request.Scheme);
           
            //ActionOnResult(false, result, user);
            var stock = _mapper.Map<Stock>(model);
            stock.Id = user.Id;
            /////
            AddStockModelToRepo.Invoke(stock,()=> {
                 _emailSender.SendEmailAsync(
                        user.Email,
                        "كود تأكيد البريد الالكترونى", $"كود التأكيد الخاص بك هو: {user.confirmCode}").Wait();
                OnFinsh.Invoke();
            });

            return GetSigningInResponseModelForStock(user,stock);
        }

        public async Task SendEmailConfirmationAsync(string email, string callbackUrl)
        {
            var body = new StringBuilder();
            body.Append($"<a href='${callbackUrl}'>confirm your email</a>");
            await _emailSender.SendEmailAsync(email, "confirm your email", body.ToString());
        }
        public async Task<bool> AddSubNewAdmin(AddNewSubAdminModel model, Action<AppUser, Admin> onAddedSuccess)
        {
            var user = new AppUser
            {
                UserName = model.UserName.Trim(),
                PhoneNumber = model.PhoneNumber
            };
            _transactionService.Begin();
            var res = await _userManager.CreateAsync(user, model.Password);
            if (!res.Succeeded)
            {
                _transactionService.RollBackChanges().End();
                throw new Exception("cannot add the default administrator");
            }

            await _userManager.AddToRoleAsync(user, Variables.adminer);
            await _userManager.AddClaimsAsync(user, new List<Claim> {
                new Claim(Variables.AdminClaimsTypes.AdminType,model.AdminType),
                new Claim(Variables.AdminClaimsTypes.Priviligs.ToString(),model.Priviligs)
            });
            var superId = _userManager.GetUserId(_httpContext.User);
            var admin = new Admin
            {
                Id = user.Id,
                Name = model.Name,
                SuperAdminId = superId
            };
            onAddedSuccess(user, admin);
            await _unitOfWork.AdminRepository.AddAsync(admin);
            _unitOfWork.Save();
            _transactionService.CommitChanges().End();
            return true;
        }
        public async Task UpdateSubAdminPassword(AppUser user, UpdateSubAdminPasswordModel model)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
        }
        public async Task<IdentityResult> UpdateSubAdminUserName(AppUser user, UpdateSubAdminUserNameModel model)
        {
            return await _userManager.SetUserNameAsync(user, model.NewUserName);
        }
        public async Task<IdentityResult> UpdateSubAdminPhoneNumber(AppUser user, UpdateSubAdminPhoneNumberModel model)
        {
            var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);
            return await _userManager.ChangePhoneNumberAsync(user, model.PhoneNumber, token);
        }
        public async Task<bool> UpdateSubAdmin(AppUser user, UpdateSubAdminModel model)
        {
            _transactionService.Begin();
            var admin = await _unitOfWork.AdminRepository.GetByIdAsync(user.Id);
            if (admin.SuperAdminId == null)
                throw new Exception("لايمكن تعديل بيانات المسؤل الرئيسى");
            admin.Name = model.Name;
            _unitOfWork.AdminRepository.Update(admin);
            if (!_unitOfWork.Save())
            {
                _transactionService.End();
                return false;
            }
            var replacedClaim = (await _userManager.GetClaimsAsync(user))
                .FirstOrDefault(c => c.Type == Variables.AdminClaimsTypes.Priviligs);
            if (replacedClaim == null) return false;
            var res = await _userManager.ReplaceClaimAsync(user, replacedClaim, new Claim(Variables.AdminClaimsTypes.Priviligs, model.Priviligs));
            if (!res.Succeeded)
            {
                _transactionService.RollBackChanges().End();
                return false;
            }
            _transactionService.CommitChanges().End();
            return true;
        }
    }
}
