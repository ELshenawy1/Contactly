using AutoMapper;
using Contactly.Core.DTOs;
using Contactly.Core.Entities;

namespace Contactly.API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Contact, ContactDTO>().ReverseMap();
            CreateMap<Contact, ContactCreateDTO>().ReverseMap();
            CreateMap<Contact, ContactUpdateDTO>().ReverseMap();
        }
    }
}
