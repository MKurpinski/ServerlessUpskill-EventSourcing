using Application.Api.Events.External.ApplicationChanged;
using AutoMapper;

namespace Application.Api.Profiles
{
    public class ApplicationToApplicationChangedEventProfile : Profile
    {
        public ApplicationToApplicationChangedEventProfile()
        {
            CreateMap<DataStorage.Models.Application, ApplicationChangedEvent>();
            CreateMap<DataStorage.Models.Address, Address> ();
            CreateMap<DataStorage.Models.FinishedSchool, FinishedSchool>();
            CreateMap<DataStorage.Models.WorkExperience, WorkExperience>();
            CreateMap<DataStorage.Models.ConfirmedSkill, ConfirmedSkill>();
        }
    }
}
