using System.ComponentModel.DataAnnotations;

namespace BookCatalog.Dto;

public class BookDto
{
    [Required, MaxLength(100, ErrorMessage = "Başlık en fazla 100 karakter olmalıdır.")]
    public string Title { get; set; }
    [Required, MaxLength(50, ErrorMessage = "Yazar adı en fazla 50 karakter olmalıdır.")]
    public string Author { get; set; }
    [Required, MaxLength(30, ErrorMessage = "Tür en fazla 30 karakter olmalıdır.")]
    public string Genre { get; set; }
    [Required, Range(1, 1000, ErrorMessage = "Sayfa sayısı 1 ile 1000 arasında olmalıdır.")]
    public int PageCount { get; set; }
}
