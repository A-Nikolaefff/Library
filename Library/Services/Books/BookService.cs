using Library.Domain;
using Library.DTO.Requests;
using Library.Repository.Interfaces;
using Library.Specifications;

namespace Library.Services.Books;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IGenreRepository _genreRepository;

    public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, IGenreRepository genreRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _genreRepository = genreRepository;
    }

    public async Task<Book> Create(CreateBookDTO bookDto)
    {
        var author = await _authorRepository.GetFirstOrDefault(new ByIdSpec<Author>(bookDto.AuthorId)) ??
                     throw new Exception("The author does not exist.");
        var genre = await _genreRepository.GetFirstOrDefault(new ByIdSpec<Genre>(bookDto.GenreId)) ??
                    throw new Exception("The genre does not exist.");
        var bookData = new Book()
        {
            Title = bookDto.Title,
            Author = author,
            Genre = genre,
            Amount = bookDto.Amount
        };
        Book book = await _bookRepository.CreateAsync(bookData);
        await _bookRepository.SaveChangesAsync();
        return book;
    }

    public async Task<IEnumerable<Book>> GetAll()
    {
        return await _bookRepository.Get(b => b.Author, b => b.Genre);
    }

    public async Task<Book?> Get(int id)
    {
        return await _bookRepository.GetFirstOrDefault(new ByIdSpec<Book>(id),
            b => b.Author, b => b.Genre);
    }

    public async Task<Book?> Update(Book bookData)
    {
        var book = await _bookRepository.GetFirstOrDefault(new ByIdSpec<Book>(bookData.Id));
        var author = await _authorRepository.GetFirstOrDefault(new ByIdSpec<Author>(bookData.AuthorId)) ??
                        throw new Exception("The author does not exist.");
        var genre = await _genreRepository.GetFirstOrDefault(new ByIdSpec<Genre>(bookData.GenreId)) ??
                      throw new Exception("The author does not exist.");
        
        if (book is null) return book;
        book.Title = bookData.Title;
        book.Author = author;
        book.Genre = genre;
        book.Amount = bookData.Amount;
        _bookRepository.Update(book);
        await _bookRepository.SaveChangesAsync();
        return book;
    }

    public async Task<Book?> Delete(int id)
    {
        Book? book = await _bookRepository.Delete(id);
        if (book is not null)
        {
            await _bookRepository.SaveChangesAsync();
        }
        return book;
    }
}