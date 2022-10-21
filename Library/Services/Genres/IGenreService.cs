using Library.Domain;
using Library.DTO.Requests;

namespace Library.Services.Genres;

public interface IGenreService
{
    Task<Genre> Create(CreateGenreDTO request);
    Task<IEnumerable<Genre>> GetAll();
    Task<Genre?> Get(int id);
    Task<Genre?> Update(UpdateGenreDTO updateGenreDto);
    Task<Genre?> Delete(int id);
}