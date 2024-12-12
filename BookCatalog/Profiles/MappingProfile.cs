using AutoMapper;
using BookCatalog.Data.Entity;
using BookCatalog.Dto;
using BookCatalog.Models;

namespace BookCatalog.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Book, BookDto>().ReverseMap();

            CreateMap<Book, BookViewModel>().ReverseMap();
        }
    }
}
