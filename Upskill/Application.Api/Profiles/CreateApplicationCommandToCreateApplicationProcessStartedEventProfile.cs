using Application.Commands.Commands;
using Application.Core.Events.CreateApplicationProcessStarted;
using AutoMapper;

namespace Application.Api.Profiles
{
    public class CreateApplicationCommandToCreateApplicationProcessStartedEventProfile : Profile
    {
        public CreateApplicationCommandToCreateApplicationProcessStartedEventProfile()
        {
            CreateMap<CreateApplicationCommand, CreateApplicationProcessStartedEvent>();
            CreateMap<Commands.Commands.Candidate.Address, Address>();
            CreateMap<Commands.Commands.Candidate.ConfirmedSkill, ConfirmedSkill>();
            CreateMap<Commands.Commands.Candidate.FinishedSchool, FinishedSchool>();
            CreateMap<Commands.Commands.Candidate.WorkExperience, WorkExperience>();
        }
    }
}
