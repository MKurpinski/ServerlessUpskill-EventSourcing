using Application.Core.Events.ApplicationChangedEvent;
using AutoMapper;

namespace Application.Core.Profiles
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
