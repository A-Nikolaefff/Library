using Library.Domain;
using Library.Repository.Interfaces;
using Library.Storage;

namespace Library.Repository.Repositories;

public class GenreRepository: Repository<Genre>, IGenreRepository
{
    public GenreRepository(LibraryContext context) : base(context)
    {

    }
}