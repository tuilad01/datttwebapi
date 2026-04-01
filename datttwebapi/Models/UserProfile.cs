using AutoMapper;

namespace datttwebapi.Models
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.EmailPhone, opt => opt.MapFrom(src => $"{src.Email} - {src.Phone}"));
        }
    }
}
