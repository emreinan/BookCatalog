using System.ComponentModel.DataAnnotations;

namespace BookCatalog.Models;

public class AddBookViewModel
{
    public string Title { get; set; }

    public string Author { get; set; }

    public string Genre { get; set; }

    public int PageCount { get; set; }
}
