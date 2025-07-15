using AutoMapper;
using BlanchisserieBackend.Models;
using BlanchisserieBackend.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Article, ArticleDto>();
        CreateMap<ClientOrder, ClientOrderDto>()
            .ForMember(
                dest => dest.Username,
                opt => opt.MapFrom(src => src.User != null ? src.User.Firstname + " " + src.User.Lastname : string.Empty)
            );;
        CreateMap<ClientOrderArticle, ClientOrderArticleDto>();
        CreateMap<Role, RoleDto>();
        CreateMap<User, UserDto>();
    }
}