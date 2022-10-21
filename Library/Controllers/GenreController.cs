using Library.Domain;
using Library.DTO.Requests;
using Library.Services.Authors;
using Library.Services.Genres;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers;
[Route("api/genres")]
public class GenreController : BaseController
{
    private readonly IGenreService _genreService;

    public GenreController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGenreDTO genreData)
    {
        var response = await _genreService.Create(genreData);
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _genreService.GetAll();
        return Ok(response);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string? id)
    {
        var response = await _genreService.Get(Convert.ToInt32(id));
        return response is not null ? Ok(response) : new NotFoundResult();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateGenreDTO updateGenreDto)
    {
        var response = await _genreService.Update(updateGenreDto);
        if (response is null) return new NotFoundResult();
        return Ok(response);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string? id)
    {
        var response = await _genreService.Delete(Convert.ToInt32(id));
        if (response is null) return new NotFoundResult();
        return Ok(response);
    }
}