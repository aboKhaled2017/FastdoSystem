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
                .ForMember(dest => dest.MgrName, o => o.MapFrom(src => src.mgrName))
                .ForMember(dest => dest.OwnerName, o => o.MapFrom(src => src.ownerName))
                .ForMember(dest => dest.AreaId, o => o.MapFrom(src => src.AreaId))
                .ForMember(dest => dest.PersPhone, o => o.MapFrom(src => src.PresPhone))
                .ForMember(dest => dest.LandlinePhone, o => o.MapFrom(src => src.LinePhone))
                .ForMember(dest => dest.Address, o => o.MapFrom(src => src.Address));
            CreateMap<StockClientRegisterModel, Stock>()
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name))
                .ForMember(dest => dest.MgrName, o => o.MapFrom(src => src.mgrName))
                .ForMember(dest => dest.OwnerName, o => o.MapFrom(src => src.ownerName))
                .ForMember(dest => dest.AreaId, o => o.MapFrom(src => src.AreaId))
                .ForMember(dest => dest.PersPhone, o => o.MapFrom(src => src.PresPhone))
                .ForMember(dest => dest.LandlinePhone, o => o.MapFrom(src => src.LinePhone))
                .ForMember(dest => dest.Address, o => o.MapFrom(src => src.Address));
        }
    }
}
