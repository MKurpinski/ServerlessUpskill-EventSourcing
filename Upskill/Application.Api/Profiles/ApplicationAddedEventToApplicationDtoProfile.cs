using Application.Api.Events.External.ApplicationAdded;
using Application.Search.Dtos;
using AutoMapper;

namespace Application.Api.Profiles
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
