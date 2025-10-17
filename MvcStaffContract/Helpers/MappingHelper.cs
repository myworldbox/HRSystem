using AutoMapper;
using MvcStaffContract.ViewModels;
using MvcStaffContract.Models;

namespace MvcStaffContract.Helpers;

public class MappingHelper : Profile
{
    public MappingHelper()
    {
        CreateMap<StaffModel, StaffViewModel>();
        CreateMap<StaffViewModel, StaffModel>();

        CreateMap<StaffViewModel, StaffModel>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => AddressHelper.CombineAddress(src.Address1, src.Address2, src.Address3, src.Address4)));

        CreateMap<StaffModel, StaffViewModel>()
            .AfterMap((src, dest) =>
            {
                var span = AddressHelper.SplitAddress(src.Address);
                dest.Address1 = span.Length > 0 ? span[0] : null;
                dest.Address2 = span.Length > 1 ? span[1] : null;
                dest.Address3 = span.Length > 2 ? span[2] : null;
                dest.Address4 = span.Length > 3 ? span[3] : null;
            });

        CreateMap<ContractModel, ContractViewModel>();
        CreateMap<ContractViewModel, ContractModel>();
    }
}