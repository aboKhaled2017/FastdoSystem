using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Repositories.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Fastdo.backendsys.Global;
using Fastdo.backendsys.Models;

namespace Fastdo.backendsys.Services.Auth
{
    public partial class AccountService
    {
        
        public async Task<bool> AddSubNewAdmin(AddNewSubAdminModel model,Action<AppUser,Admin> onAddedSuccess)
        {
            var user = new AppUser
            {
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber
            };
            _transactionService.Begin();
            var res = await _userManager.CreateAsync(user,model.Password);
            if (!res.Succeeded)
            {
                _transactionService.RollBackChanges().End();
                throw new Exception("cannot add the default administrator");
            }

            await _userManager.AddToRoleAsync(user, Variables.adminer);
            await _userManager.AddClaimsAsync(user, new List<Claim> {
                new Claim(Variables.AdminClaimsTypes.AdminType,model.AdminType),
                new Claim(Variables.AdminClaimsTypes.Previligs,model.Previligs)
            }); 
            var superId = _userManager.GetUserId(_httpContext.User);
            var admin = new Admin
            {
                Id=user.Id,
                Name =model.Name,
                SuperAdminId=superId
            };
            onAddedSuccess(user,admin);
            if( await _adminRepository.AddAsync(admin))
            {
                _transactionService.CommitChanges().End();
                return true;
            }
            _transactionService.RollBackChanges().End();
            return false;
        }
        public async Task UpdateSubAdminPassword(AppUser user,UpdateSubAdminPasswordModel model)
        {
            var token =await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
        }
        public async Task<IdentityResult> UpdateSubAdminUserName(AppUser user, UpdateSubAdminUserNameModel model)
        {
          return await _userManager.SetUserNameAsync(user, model.NewUserName);          
        }
        public async Task<IdentityResult> UpdateSubAdminPhoneNumber(AppUser user, UpdateSubAdminPhoneNumberModel model)
        {
            var token =await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);
            return await _userManager.ChangePhoneNumberAsync(user,model.PhoneNumber,token);
        }
        public async Task<bool> UpdateSubAdmin(AppUser user, UpdateSubAdminModel model)
        {
            _transactionService.Begin();
            var admin = await _adminRepository.GetByIdAsync(user.Id);
            admin.Name = model.Name;
            _adminRepository.Update(admin);
            if(!await _adminRepository.SaveAsync())
            {
                _transactionService.End();
                return false;
            }
            var replacedClaim = (await _userManager.GetClaimsAsync(user))
                .FirstOrDefault(c => c.Type == Variables.AdminClaimsTypes.Previligs);
            if (replacedClaim == null) return false;
            var res = await _userManager.ReplaceClaimAsync(user, replacedClaim, new Claim(Variables.AdminClaimsTypes.Previligs, model.Previligs));
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
