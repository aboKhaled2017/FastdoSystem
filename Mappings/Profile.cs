using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;
using System_Back_End.InitSeeds.Helpers;
using System_Back_End.Models;

namespace System_Back_End.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PharmacierObjectSeeder, Pharmacy>();

            CreateMap<AppUser, PharmacyClientResponseModel>();
            CreateMap<Pharmacy,PharmacyClientResponseModel>();
            CreateMap<AppUser, StockClientResponseModel>();
            CreateMap<Stock,   StockClientResponseModel>();

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
        }
    }
}
