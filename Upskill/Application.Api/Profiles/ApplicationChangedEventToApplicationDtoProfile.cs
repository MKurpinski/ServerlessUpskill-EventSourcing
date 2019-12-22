using Application.Api.Events.External.ApplicationChanged;
using Application.Search.Dtos;
using AutoMapper;

namespace Application.Api.Profiles
{
    public class ApplicationChangedEventToApplicationDtoProfile : Profile
    {
        public ApplicationChangedEventToApplicationDtoProfile()
        {
            CreateMap<ApplicationChangedEvent, ApplicationDto>();
            CreateMap<Address, AddressDto>();
            CreateMap<FinishedSchool, FinishedSchoolDto>();
            CreateMap<WorkExperience, WorkExperienceDto>();
            CreateMap<ConfirmedSkill, ConfirmedSkillDto>();
        }
    }
}
