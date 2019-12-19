using Application.Commands.Commands.Candidate;
using Application.RequestMappers.Dtos;
using AutoMapper;

namespace Application.Commands.Profiles
{
    public class CandidateDtoToCandidateProfile : Profile
    {
        public CandidateDtoToCandidateProfile()
        {
            CreateMap<CandidateDto, Candidate>();
            CreateMap<AddressDto, Address>();
            CreateMap<ConfirmedSkillDto, ConfirmedSkill>();
            CreateMap<FinishedSchoolDto, FinishedSchool>();
            CreateMap<WorkExperienceDto, WorkExperience>();
        }
    }
}
