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
        Log.Information($"New book added: {bookDto.Title}", bookDto);
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

        var updatedBook = mapper.Map<Book>(bookDto);
        updatedBook.Id = id;

        dbContext.Books.Update(updatedBook);
        dbContext.SaveChanges();
        Log.Information($"Book updated: {bookDto.Title}", bookDto);
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

}
