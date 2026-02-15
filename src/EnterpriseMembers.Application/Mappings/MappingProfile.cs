using AutoMapper;
using EnterpriseMembers.Application.DTOs;
using EnterpriseMembers.Domain.Entities;
using EnterpriseMembers.Domain.Enums;

namespace EnterpriseMembers.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Member, MemberDto>()
            .ForMember(dest => dest.MembershipType, opt => opt.MapFrom(src => src.MembershipType.ToString()))
            .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate.ToString("yyyy-MM-dd")));

        CreateMap<CreateMemberDto, Member>()
            .ForMember(dest => dest.MembershipType, opt => opt.MapFrom(src => Enum.Parse<MembershipType>(src.MembershipType, true)));

        CreateMap<UpdateMemberDto, Member>()
            .ForMember(dest => dest.MembershipType, opt => opt.MapFrom(src => Enum.Parse<MembershipType>(src.MembershipType, true)));
    }
}
