using Application.Commands.Commands;
using Application.Core.Events.ApplicationChangedEvent;
using AutoMapper;

namespace Application.Api.Profiles
{
    public class SaveApplicationCommandToApplicationChangedEventProfile : Profile
    {
        public SaveApplicationCommandToApplicationChangedEventProfile()
        {
            CreateMap<SaveApplicationCommand, ApplicationChangedEvent>();
            CreateMap<Commands.Commands.Candidate.Address, Address>();
            CreateMap<Commands.Commands.Candidate.ConfirmedSkill, ConfirmedSkill>();
            CreateMap<Commands.Commands.Candidate.FinishedSchool, FinishedSchool>();
            CreateMap<Commands.Commands.Candidate.WorkExperience, WorkExperience>();
        }
    }
}
