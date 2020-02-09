using Application.Search.Dtos;
using AutoMapper;

namespace Application.Core.Profiles
{
    public class ApplicationToApplicationDto : Profile
    {
        public ApplicationToApplicationDto()
        {
            CreateMap<Aggregates.Application, ApplicationDto>();
            CreateMap<ValueObjects.Address, AddressDto>();
            CreateMap<ValueObjects.FinishedSchool, FinishedSchoolDto>();
            CreateMap<ValueObjects.WorkExperience, WorkExperienceDto>();
            CreateMap<ValueObjects.ConfirmedSkill, ConfirmedSkillDto>();
        }
    }
}
