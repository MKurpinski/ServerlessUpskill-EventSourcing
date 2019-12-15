using Application.Search.Dtos;
using Application.Search.Models;
using AutoMapper;

namespace Application.Search.Profiles
{
    public class SearchableApplicationToApplicationDtoProfile : Profile
    {
        public SearchableApplicationToApplicationDtoProfile()
        {
            CreateMap<SearchableApplication, SimpleApplicationDto>();
            CreateMap<SearchableApplication, ApplicationDto>()
                .ReverseMap();
            CreateMap<SearchableAddress, AddressDto>()
                .ReverseMap();
            CreateMap<SearchableConfirmedSkill, ConfirmedSkillDto>()
                .ReverseMap();
            CreateMap<SearchableFinishedSchool, FinishedSchoolDto>()
                .ReverseMap();
            CreateMap<SearchableWorkExperience, WorkExperienceDto>()
                .ReverseMap();
        }
    }
}
