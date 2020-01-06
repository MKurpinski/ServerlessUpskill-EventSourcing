using Application.Core.Events.ApplicationChangedEvent;
using Application.Search.Dtos;
using AutoMapper;

namespace Application.Core.Profiles
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
