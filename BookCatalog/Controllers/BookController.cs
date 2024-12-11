using AutoMapper;
using BookCatalog.Data.Context;
using BookCatalog.Data.Entity;
using BookCatalog.Dto;
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
        return View(books);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await dbContext.Books.FindAsync(id);
        if (book is null)
        {
            ViewData["ErrorMessage"] = "Book not found.";
            return View("Error"); 
        }
        return View(book);
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
        Log.Information($"New book added: {bookDto}", bookDto);
        return RedirectToAction(nameof(Get));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var book = dbContext.Books.Find(id);
        if (book is null)
        {
            ViewData["ErrorMessage"] = "Book not found.";
            return View("Error");
        }
        return View(book);
    }

    [HttpPost]
    public IActionResult Edit(int id, BookDto bookDto)
    {
        if (!ModelState.IsValid)
        {
            Log.Warning("Validation error");
            return View(bookDto);
        }

        var book = mapper.Map<Book>(bookDto);
        dbContext.Books.Update(book);
        dbContext.SaveChanges();
        Log.Information($"Book updated: {bookDto}", bookDto);
        return RedirectToAction(nameof(Get));
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var book = dbContext.Books.Find(id);
        if (book is null)
        {
            return NotFound();
        }

        dbContext.Books.Remove(book);
        dbContext.SaveChanges();
        Log.Information($"Book deleted: {book}", book);

        return RedirectToAction(nameof(Get));
    }

}
