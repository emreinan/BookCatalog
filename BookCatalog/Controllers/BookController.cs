using AutoMapper;
using BookCatalog.Data.Context;
using BookCatalog.Data.Entity;
using BookCatalog.Dto;
using BookCatalog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BookCatalog.Controllers;

public class BookController(ApplicationDbContext dbContext,IMapper mapper) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var books = await dbContext.Books.ToListAsync();
        var bookListViewModel = mapper.Map<List<BookViewModel>>(books);
        return View(bookListViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await dbContext.Books.FindAsync(id);
        if (book is null)
        {
            return View("Error", "Book not found.");
        }
        var bookViewModel = mapper.Map<BookViewModel>(book);
        return View(bookViewModel);
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Add(BookDto bookDto)
    {
        if (!ModelState.IsValid)
        {
            Log.Warning("Validation error");
            return View(bookDto);
        }

        var book = mapper.Map<Book>(bookDto);
        dbContext.Books.Add(book);
        dbContext.SaveChanges();

        //AddedBookWithReflection(bookDto);
        Log.Information($"New book added: Title {bookDto.Title} Author {bookDto.Author} " +
            $"Genre {bookDto.Genre} PageCount {bookDto.PageCount}" , bookDto);
        return RedirectToAction(nameof(Get));
    }

    

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var book = dbContext.Books.Find(id);
        if (book is null)
        {
            return View("Error", "Book not found.");
        }
        var bookViewModel = mapper.Map<BookViewModel>(book);
        return View(bookViewModel);
    }

    [HttpPost]
    public IActionResult Edit(int id, BookDto bookDto)
    {
        if (!ModelState.IsValid)
        {
            Log.Warning("Validation error");
            return View(bookDto);
        }

        var book = dbContext.Books.Find(id);
        if (book is null)
        {
            return View("Error", "Book not found.");
        }
        CheckIfUpdated(bookDto,book);

        var updatedBook = mapper.Map<Book>(bookDto);
        dbContext.Books.Update(updatedBook);
        dbContext.SaveChanges();
        return RedirectToAction(nameof(Get));
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var book = dbContext.Books.Find(id);
        if (book is null)
        {
            return View("Error", "Book not found.");
        }

        dbContext.Books.Remove(book);
        dbContext.SaveChanges();
        Log.Information($"Book deleted: {book.Title}", book);

        return RedirectToAction(nameof(Get));
    }

    private static void CheckIfUpdated(BookDto bookDto, Book book)
    {
        var changes = new List<string>();
        var bookProperties = typeof(Book).GetProperties();
        var bookDtoProperties = typeof(BookDto).GetProperties();

        foreach (var dtoProperty in bookDtoProperties)
        {
            var bookProperty = bookProperties.FirstOrDefault(p => p.Name == dtoProperty.Name);
            if (bookProperty != null)
            {
                var oldValue = bookProperty.GetValue(book);
                var newValue = dtoProperty.GetValue(bookDto);

                if (!object.Equals(oldValue, newValue))
                {
                    changes.Add($"{dtoProperty.Name}: '{oldValue}' -> '{newValue}'");
                }
            }
        }

        if (changes.Any())
        {
            Log.Information($"Book named '{book.Title}' updated: {string.Join(", ", changes)}");
        }

    }

    private static void AddedBookWithReflection(BookDto bookDto)
    {
        var propertyChanges = new List<string>();

        foreach (var property in typeof(BookDto).GetProperties())
        {
            var value = property.GetValue(bookDto);
            propertyChanges.Add($"{property.Name}: {value}");
        }

        var bookDetails = string.Join(", ", propertyChanges);
        Log.Information($"New book added: {bookDetails}");
    }

}
