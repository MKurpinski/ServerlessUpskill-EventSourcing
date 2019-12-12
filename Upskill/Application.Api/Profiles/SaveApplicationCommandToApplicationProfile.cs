using Application.Commands.Commands;
using AutoMapper;

namespace Application.Api.Profiles
{
    public class SaveApplicationCommandToApplicationProfile : Profile
    {
        public SaveApplicationCommandToApplicationProfile()
        {
            CreateMap<SaveApplicationCommand, DataStorage.Model.Application>();
            CreateMap<Commands.Commands.Candidate.Address, DataStorage.Model.Address>();
            CreateMap<Commands.Commands.Candidate.ConfirmedSkill, DataStorage.Model.ConfirmedSkill>();
            CreateMap<Commands.Commands.Candidate.FinishedSchool, DataStorage.Model.FinishedSchool>();
            CreateMap<Commands.Commands.Candidate.WorkExperience, DataStorage.Model.WorkExperience>();
        }
    }
}
