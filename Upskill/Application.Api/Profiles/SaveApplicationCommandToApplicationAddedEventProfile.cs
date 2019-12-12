using Application.Api.Events.External.ApplicationAdded;
using Application.Commands.Commands;
using AutoMapper;

namespace Application.Api.Profiles
{
    public class SaveApplicationCommandToApplicationAddedEventProfile : Profile
    {
        public SaveApplicationCommandToApplicationAddedEventProfile()
        {
            CreateMap<SaveApplicationCommand, ApplicationAddedEvent>();
            CreateMap<Commands.Commands.Candidate.Address, Address>();
            CreateMap<Commands.Commands.Candidate.ConfirmedSkill, ConfirmedSkill>();
            CreateMap<Commands.Commands.Candidate.FinishedSchool, FinishedSchool>();
            CreateMap<Commands.Commands.Candidate.WorkExperience, WorkExperience>();
        }
    }
}
