using AutoMapper;
using HRSystem.Domain.Entities;
using HRSystem.Application.ViewModels;

namespace HRSystem.Application.Helpers;

public class MappingHelper : Profile
{
    public MappingHelper()
    {
        CreateMap<Staff, StaffViewModel>().ReverseMap();

        CreateMap<Staff, StaffViewModel>()
            .AfterMap((src, dest) =>
            {
                var span = AddressHelper.SplitAddress(src.Address);
                dest.Address1 = span.Length > 0 ? span[0] : null;
                dest.Address2 = span.Length > 1 ? span[1] : null;
                dest.Address3 = span.Length > 2 ? span[2] : null;
                dest.Address4 = span.Length > 3 ? span[3] : null;
            })
            .ReverseMap()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => AddressHelper.CombineAddress(src.Address1, src.Address2, src.Address3, src.Address4)));

        CreateMap<Contract, ContractViewModel>().ReverseMap();
    }
}