using Library.Domain;
using Library.DTO.Requests;
using Library.Services.Authors;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers;
[Route("api/authors")]
public class AuthorController : BaseController
{
    private readonly IAuthorService _authorService;

    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAuthorDTO authorData)
    {
        var response = await _authorService.Create(authorData);
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _authorService.GetAll();
        return Ok(response);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string? id)
    {
        var response = await _authorService.Get(Convert.ToInt32(id));
        return response is not null ? Ok(response) : new NotFoundResult();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateAuthorDTO updateAuthorDto)
    {
        var response = await _authorService.Update(updateAuthorDto);
        if (response is null) return new NotFoundResult();
        return Ok(response);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string? id)
    {
        var response = await _authorService.Delete(Convert.ToInt32(id));
        if (response is null) return new NotFoundResult();
        return Ok(response);
    }
}