using Library.Domain;
using Library.DTO.Requests;
using Library.Services.Books;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers;
[Route("api/books")]
public class BookController : BaseController
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookDTO bookData)
    {
        var response = await _bookService.Create(bookData);
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _bookService.GetAll();
        return Ok(response);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string? id)
    {
        var response = await _bookService.Get(Convert.ToInt32(id));
        return response is not null ? Ok(response) : new NotFoundResult();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Book bookData)
    {
        var response = await _bookService.Update(bookData);
        if (response is null) return new NotFoundResult();
        return Ok(response);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string? id)
    {
        var response = await _bookService.Delete(Convert.ToInt32(id));
        if (response is null) return new NotFoundResult();
        return Ok(response);
    }
}