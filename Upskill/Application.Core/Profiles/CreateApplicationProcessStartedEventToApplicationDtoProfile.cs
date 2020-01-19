using Application.Core.Events.CreateApplicationProcessStarted;
using Application.Search.Dtos;
using AutoMapper;

namespace Application.Core.Profiles
{
    public class CreateApplicationProcessStartedEventToApplicationDtoProfile : Profile
    {
        public CreateApplicationProcessStartedEventToApplicationDtoProfile()
        {
            CreateMap<CreateApplicationProcessStartedEvent, ApplicationDto>();
            CreateMap<Address, AddressDto>();
            CreateMap<FinishedSchool, FinishedSchoolDto>();
            CreateMap<WorkExperience, WorkExperienceDto>();
            CreateMap<ConfirmedSkill, ConfirmedSkillDto>();
        }
    }
}
