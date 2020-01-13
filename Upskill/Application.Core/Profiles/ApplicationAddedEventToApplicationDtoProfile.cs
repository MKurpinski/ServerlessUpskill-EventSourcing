using Application.Core.Events.ApplicationAddedEvent;
using Application.Search.Dtos;
using AutoMapper;

namespace Application.Core.Profiles
{
    public class ApplicationAddedEventToApplicationDtoProfile : Profile
    {
        public ApplicationAddedEventToApplicationDtoProfile()
        {
            CreateMap<ApplicationAddedEvent, ApplicationDto>();
            CreateMap<Address, AddressDto>();
            CreateMap<FinishedSchool, FinishedSchoolDto>();
            CreateMap<WorkExperience, WorkExperienceDto>();
            CreateMap<ConfirmedSkill, ConfirmedSkillDto>();
        }
    }
}
