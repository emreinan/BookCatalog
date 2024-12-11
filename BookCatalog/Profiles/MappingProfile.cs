using AutoMapper;
using BookCatalog.Data.Entity;
using BookCatalog.Dto;

namespace BookCatalog.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Book, BookDto>().ReverseMap();
        }
    }
}
