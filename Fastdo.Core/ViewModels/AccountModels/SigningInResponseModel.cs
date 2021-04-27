using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.Core.ViewModels
{
    public interface ISigningResponseModel { }
    public class SigningAdministratorClientInResponseModel : ISigningResponseModel
    {
        public AdministratorClientResponseModel user { get; set; }
        public TokenModel accessToken { get; set; }
    }
    public class SigningPharmacyClientInResponseModel:ISigningResponseModel
    {
        public PharmacyClientResponseModel user { get; set; }
        public TokenModel accessToken { get; set; }
    }
    public class SigningStockClientInResponseModel : ISigningResponseModel
    {
        public StockClientResponseModel user { get; set; }
        public TokenModel accessToken { get; set; }
    }
}
