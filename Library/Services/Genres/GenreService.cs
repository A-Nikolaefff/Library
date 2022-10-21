using Library.Domain;
using Library.DTO.Requests;
using Library.Repository.Interfaces;
using Library.Specifications;

namespace Library.Services.Genres;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;
    
    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<Genre> Create(CreateGenreDTO request)
    {
        var genreData = new Genre()
        {
            Name = request.Name
        };
        Genre genre = await _genreRepository.CreateAsync(genreData);
        await _genreRepository.SaveChangesAsync();
        return genre;
    }

    public async Task<IEnumerable<Genre>> GetAll()
    {
        return await _genreRepository.Get();
    }

    public async Task<Genre?> Get(int id)
    {
        return await _genreRepository.GetFirstOrDefault(new ByIdSpec<Genre>(id));
    }

    public async Task<Genre?> Update(UpdateGenreDTO updateGenreDto)
    {
        var genre = await _genreRepository.GetFirstOrDefault(new ByIdSpec<Genre>(updateGenreDto.Id));
        if (genre is null) return genre;
        genre.Name = updateGenreDto.Name;
        _genreRepository.Update(genre);
        await _genreRepository.SaveChangesAsync();
        return genre;
    }

    public async Task<Genre?> Delete(int id)
    {
        Genre? genre = await _genreRepository.Delete(id);
        if (genre is not null)
        {
            await _genreRepository.SaveChangesAsync();
        }
        return genre;
    }
}