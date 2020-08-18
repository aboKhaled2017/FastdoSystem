using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.Repositories.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text;
using Fastdo.backendsys.Models;
using AutoMapper;
using Fastdo.backendsys.Repositories;

namespace Fastdo.backendsys.Services.Auth
{
    public partial class AccountService
    {
        #region constructor and properties
        private readonly JWThandlerService _jWThandlerService;
        private readonly IEmailSender _emailSender;

        public IAdminRepository _adminRepository { get; }
        private IPharmacyRepository _pharmacyRepository { get; }
        private IStockRepository _stockRepository { get;}

        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public TransactionService _transactionService { get; }
        private HttpContext _httpContext { get; set; }
        private IUrlHelper _Url { get; set; }
        private readonly IConfigurationSection _JWT = RequestStaticServices.GetConfiguration().GetSection("JWT");

        public AccountService(
            IEmailSender emailSender,
            JWThandlerService jWThandlerService,
            UserManager<AppUser> userManager,
            IMapper mapper,
            IAdminRepository adminRepository,
            IStockRepository stockRepository,
            IPharmacyRepository pharmacyRepository,
            TransactionService transactionService)
        {
            _jWThandlerService = jWThandlerService;
            _userManager = userManager;
            _emailSender = emailSender;
            _adminRepository = adminRepository;
            _pharmacyRepository = pharmacyRepository;
            _stockRepository = stockRepository;
            _mapper = mapper;
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
            _user.Prevligs = claims.FirstOrDefault(c => c.Type == Variables.AdminClaimsTypes.Previligs).Value;
            return new SigningAdministratorClientInResponseModel
            {
                user = _user,
                accessToken = _jWThandlerService.CreateAccessToken_ForAdministartor(user, admin.Name, claims)
            };

        }
        public ISigningResponseModel GetSigningInResponseModelForPharmacy(AppUser user, Pharmacy pharmacy)
        {
                return new SigningPharmacyClientInResponseModel
                {
                    user = _mapper.MergeInto<PharmacyClientResponseModel>(user, pharmacy),
                    accessToken = _jWThandlerService.CreateAccessToken(user,Variables.pharmacier,pharmacy.Name)
                };   
                      
        }
        public ISigningResponseModel GetSigningInResponseModelForStock(AppUser user, Stock stock)
        {
            return new SigningStockClientInResponseModel
            {
                user = _mapper.MergeInto<StockClientResponseModel>(user, stock),
                accessToken = _jWThandlerService.CreateAccessToken(user, Variables.stocker,stock.Name)
            };
        }

        
        public async Task<ISigningResponseModel> GetSigningInResponseModelForCurrentUser(AppUser user)
        {
            var userType = Functions.CurrentUserType();
            if (userType == UserType.pharmacier)
            {
                var pharmacy = await _pharmacyRepository.GetByIdAsync(user.Id);
                return GetSigningInResponseModelForPharmacy(user, pharmacy);
            }
            else
            {
                var stock = await _stockRepository.GetByIdAsync(user.Id);
                return GetSigningInResponseModelForStock(user, stock);
            }
        }
        public async Task<ISigningResponseModel> GetSigningInResponseModelForAdministrator(AppUser user,string adminType)
        {
            var admin = await _adminRepository.GetByIdAsync(user.Id);
            return await GetSigningInResponseModelForAdministrator(user, admin, adminType);
        }
        public async Task<ISigningResponseModel> GetSigningInResponseModelForPharmacy(AppUser user)
        {
            var pharmacy = await _pharmacyRepository.GetByIdAsync(user.Id);
            return GetSigningInResponseModelForPharmacy(user, pharmacy);
        }

        public async Task<ISigningResponseModel> GetSigningInResponseModelForStock(AppUser user)
        {
            var stock = await _stockRepository.GetByIdAsync(user.Id);
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
                confirmCode = Functions.GenerateConfirmationTokenCode()
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
        public async Task<ISigningResponseModel> SignUpStockAsync(StockClientRegisterModel model,IExecuterDelayer executerDelayer)
        {
            //the email is already checked at validation if it was existed before for any user
            var user = new AppUser {
                UserName = model.Email,
                Email = model.Email, 
                PhoneNumber = model.PersPhone,
                confirmCode=Functions.GenerateConfirmationTokenCode() };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return null;
                result = await _userManager.AddToRoleAsync(user, Variables.stocker);
            if (!result.Succeeded)
                return null;
            //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var callbackUrl = _Url.EmailConfirmationLink(user.Id.ToString(), code, _httpContext.Request.Scheme);
            executerDelayer.OnExecuting = async() =>
            {
                await _emailSender.SendEmailAsync(
                    user.Email,
                    "كود تأكيد البريد الالكترونى", $"كود التأكيد الخاص بك هو: {user.confirmCode}");
            };
            //ActionOnResult(false, result, user);
            var stock = _mapper.Map<Stock>(model);
            stock.Id = user.Id;
            return GetSigningInResponseModelForStock(user,stock);
        }

        public async Task SendEmailConfirmationAsync(string email, string callbackUrl)
        {
            var body = new StringBuilder();
            body.Append($"<a href='${callbackUrl}'>confirm your email</a>");
            await _emailSender.SendEmailAsync(email, "confirm your email", body.ToString());
        }
    }
}
