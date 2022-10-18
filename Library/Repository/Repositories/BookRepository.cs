using Library.Domain;
using Library.Repository.Interfaces;
using Library.Storage;

namespace Library.Repository.Repositories;

public class BookRepository : Repository<Book>, IBookRepository
{
    public BookRepository(LibraryContext context) : base(context)
    {

    }
}