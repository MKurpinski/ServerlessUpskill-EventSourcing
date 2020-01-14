using Application.Commands.Commands;
using Application.Core.Events.ApplicationAddedEvent;
using AutoMapper;

namespace Application.Api.Profiles
{
    public class SaveApplicationCommandToApplicationAddedEventProfile : Profile
    {
        public SaveApplicationCommandToApplicationAddedEventProfile()
        {
            CreateMap<SaveApplicationCommand, ApplicationAddedEvent>()
                .ForMember(dest => dest.CorrelationId, opts => opts.MapFrom(src => src.Id));
            CreateMap<Commands.Commands.Candidate.Address, Address>();
            CreateMap<Commands.Commands.Candidate.ConfirmedSkill, ConfirmedSkill>();
            CreateMap<Commands.Commands.Candidate.FinishedSchool, FinishedSchool>();
            CreateMap<Commands.Commands.Candidate.WorkExperience, WorkExperience>();
        }
    }
}
