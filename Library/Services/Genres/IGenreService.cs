using Library.Domain;
using Library.DTO.Requests;

namespace Library.Services.Genres;

public interface IGenreService
{
    Task<Genre> Create(CreateGenreDTO request);
    Task<IEnumerable<Genre>> GetAll();
    Task<Genre?> Get(int id);
    Task<Genre?> Update(Genre genreData);
    Task<Genre?> Delete(int id);
}