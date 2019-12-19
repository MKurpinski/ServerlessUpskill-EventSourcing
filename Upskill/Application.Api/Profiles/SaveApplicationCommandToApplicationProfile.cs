using Application.Commands.Commands;
using Application.DataStorage.Models;
using AutoMapper;

namespace Application.Api.Profiles
{
    public class SaveApplicationCommandToApplicationProfile : Profile
    {
        public SaveApplicationCommandToApplicationProfile()
        {
            CreateMap<SaveApplicationCommand, DataStorage.Models.Application>();
            CreateMap<Commands.Commands.Candidate.Address, Address>();
            CreateMap<Commands.Commands.Candidate.ConfirmedSkill, ConfirmedSkill>();
            CreateMap<Commands.Commands.Candidate.FinishedSchool, FinishedSchool>();
            CreateMap<Commands.Commands.Candidate.WorkExperience, WorkExperience>();
        }
    }
}
