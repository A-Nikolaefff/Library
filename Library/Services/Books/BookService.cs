using Library.Domain;
using Library.DTO.Requests;
using Library.Repository.Interfaces;
using Library.Specifications;
using Serilog;

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

    public async Task<Book?> Create(CreateBookDTO createBookDto)
    {
        Author? author;
        Genre? genre;
        try
        {
            author = await _authorRepository.GetFirstOrDefault(new ByIdSpec<Author>(createBookDto.AuthorId)) ??
                         throw new ArgumentException("the author does not exist.");
            genre = await _genreRepository.GetFirstOrDefault(new ByIdSpec<Genre>(createBookDto.GenreId)) ??
                        throw new ArgumentException("the genre does not exist.");
        }
        catch (ArgumentException e)
        {
            Log.Error("Book creation error: {Message}", e.Message);
            return null;
        }
        var bookData = new Book(createBookDto.Title, author, genre, createBookDto.Amount);
        var book = await _bookRepository.CreateAsync(bookData);
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

    public async Task<Book?> Update(UpdateBookDTO updateBookDto)
    {
        Book? book;
        Author? author;
        Genre? genre;
        try
        {
            book = await _bookRepository.GetFirstOrDefault(new ByIdSpec<Book>(updateBookDto.Id)) ??
                       throw new ArgumentException("the book does not exist.");
            author = await _authorRepository.GetFirstOrDefault(new ByIdSpec<Author>(updateBookDto.AuthorId)) ??
                         throw new ArgumentException("new author does not exist.");
            genre = await _genreRepository.GetFirstOrDefault(new ByIdSpec<Genre>(updateBookDto.GenreId)) ??
                        throw new ArgumentException("new genre does not exist.");
        }
        catch (ArgumentException e)
        {
            Log.Error("Book update error: {Message}", e.Message);
            return null; }
        book.Title = updateBookDto.Title;
        book.Author = author;
        book.Genre = genre;
        book.Amount = updateBookDto.Amount;
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