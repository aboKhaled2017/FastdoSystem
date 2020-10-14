using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.backendsys.InitSeeds.Helpers;
using Fastdo.backendsys.Models;
using Fastdo.backendsys.Controllers.Stocks;
using Fastdo.backendsys.Controllers.Pharmacies;
using Newtonsoft.Json;

namespace Fastdo.backendsys.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PharmacierObjectSeeder, Pharmacy>();

            CreateMap<AppUser, PharmacyClientResponseModel>();
            CreateMap<Pharmacy, PharmacyClientResponseModel>()
                .ForMember(dest => dest.UserType, o => o.MapFrom(s => UserType.pharmacier));
            CreateMap<AppUser, StockClientResponseModel>();
            CreateMap<Stock,   StockClientResponseModel>()
                .ForMember(d=>d.PharmasClasses,o=>o.Ignore())
                .ForMember(dest => dest.UserType, o => o.MapFrom(s => UserType.stocker));
            CreateMap<AppUser, AdministratorClientResponseModel>();
            CreateMap<Admin, AdministratorClientResponseModel>();


            CreateMap<PharmacyClientRegisterModel, Pharmacy>()
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name))
                .ForMember(dest => dest.MgrName, o => o.MapFrom(src => src.MgrName))
                .ForMember(dest => dest.OwnerName, o => o.MapFrom(src => src.OwnerName))
                .ForMember(dest => dest.AreaId, o => o.MapFrom(src => src.AreaId))
                .ForMember(dest => dest.PersPhone, o => o.MapFrom(src => src.PersPhone))
                .ForMember(dest => dest.LandlinePhone, o => o.MapFrom(src => src.LinePhone))
                .ForMember(dest => dest.Address, o => o.MapFrom(src => src.Address));
            CreateMap<StockClientRegisterModel, Stock>()
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name))
                .ForMember(dest => dest.MgrName, o => o.MapFrom(src => src.MgrName))
                .ForMember(dest => dest.OwnerName, o => o.MapFrom(src => src.OwnerName))
                .ForMember(dest => dest.AreaId, o => o.MapFrom(src => src.AreaId))
                .ForMember(dest => dest.PersPhone, o => o.MapFrom(src => src.PersPhone))
                .ForMember(dest => dest.LandlinePhone, o => o.MapFrom(src => src.LinePhone))
                .ForMember(dest => dest.Address, o => o.MapFrom(src => src.Address));

            CreateMap<Phr_RegisterModel_Contacts, Pharmacy>();
            CreateMap<Phr_RegisterModel_Contacts, Stock>();
            CreateMap<Phr_Contacts_Update, Pharmacy>();
            CreateMap<Stk_Contacts_Update, Stock>();

            CreateMap<ComplainToAddModel, Complain>();

            CreateMap<AddLzDrugModel, LzDrug>();
            CreateMap<LzDrug, LzDrugModel_BM>();
            CreateMap<UpdateLzDrugModel, LzDrug>();

            CreateMap<LzDrugRequest,LzDrgRequest_ForUpdate_BM>();
            CreateMap<LzDrgRequest_ForUpdate_BM, LzDrugRequest>();

            CreateMap<LzDrugRequest, LzDrgRequest_ForUpdate_BM>();
            CreateMap<LzDrgRequest_ForUpdate_BM, LzDrugRequest>();

            CreateMap<Pharmacy, Pharmacy_Update_ADM_Model>();
            CreateMap<Pharmacy_Update_ADM_Model, Pharmacy>();

            CreateMap<Stock_Update_ADM_Model, Stock>();
            CreateMap<Stock, Stock_Update_ADM_Model>();

            CreateMap<PharmacyInStock, HandlePharmaRequestModel>()
                .ForMember(dest => dest.Status, o => o.MapFrom(s => s.PharmacyReqStatus));
            CreateMap<HandlePharmaRequestModel, PharmacyInStock>()
                .ForMember(dest => dest.PharmacyReqStatus, o => o.MapFrom(s => s.Status));
 

            /*CreateMap<LzDrug, LzDrugCard_Info_BM>();
            CreateMap<Pharmacy,LzDrugCard_Info_BM>()
                .ForMember(dest=>dest.PharmName)*/
        }
    }
}
