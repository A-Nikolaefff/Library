using Library.Domain;
using Library.Repository.Interfaces;
using Library.Storage;

namespace Library.Repository.Repositories;

public class AuthorRepository : Repository<Author>, IAuthorRepository
{
    public AuthorRepository(LibraryContext context) : base(context)
    {

    }
}
