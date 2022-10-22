using Library.Domain;
using Library.DTO.Requests;
using Library.Repository.Interfaces;
using Library.Services.Authors;
using Library.Services.Books;
using Library.Services.Genres;
using Library.Specifications;
using LibraryTests.MockBuilders;

namespace LibraryTests;

public class BookServiceTests
{
    private static readonly Fixture Fixture;

    static BookServiceTests()
    {
        Fixture = new Fixture();
        Fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
    
    private readonly List<Author> _expectedAuthors;
    
    private readonly List<Genre> _expectedGenres;
    
    private readonly List<Book> _expectedBooks;

    public BookServiceTests()
    {
        _expectedAuthors = new List<Author>()
        {
            Fixture.Create<Author>(),
            Fixture.Create<Author>(),
            Fixture.Create<Author>(),
        };
        _expectedGenres = new List<Genre>()
        {
            Fixture.Create<Genre>(),
            Fixture.Create<Genre>(),
            Fixture.Create<Genre>(),
        };
        _expectedBooks = new List<Book>()
        {
            // Creation books with existing authors and genres
            new Book(Fixture.Create<string>(), _expectedAuthors[0], 
                _expectedGenres[0], Fixture.Create<int>()) {Id = Fixture.Create<int>()},
            new Book(Fixture.Create<string>(), _expectedAuthors[1], 
                _expectedGenres[2], Fixture.Create<int>()) {Id = Fixture.Create<int>()},
            new Book(Fixture.Create<string>(), _expectedAuthors[0], 
                _expectedGenres[1], Fixture.Create<int>()) {Id = Fixture.Create<int>()}
        };
    }

    private IBookService GetDefaultBookService()
    {
        var authorRepositoryMock = AuthorRepositoryMockBuilder.Create(_expectedAuthors);
        var genreRepositoryMock = GenreRepositoryMockBuilder.Create(_expectedGenres);
        var bookRepositoryMock = BookRepositoryMockBuilder.Create(_expectedBooks);
        return new BookService(bookRepositoryMock.Object, authorRepositoryMock.Object, genreRepositoryMock.Object);
    }
    
    [Fact]
    public async void GIVEN_correct_CreateBookDto_WHEN_Create_method_is_invoked_THEN_correct_Book_is_returned()
    {
        // Arrange
        var bookService = GetDefaultBookService();
        var formerBookRepositoryLength = _expectedBooks.Count;
        var createBookDto = new CreateBookDTO()
        {
            Title = Fixture.Create<string>(),
            AuthorId = _expectedAuthors[0].Id,
            GenreId = _expectedGenres[0].Id,
            Amount = Fixture.Create<int>()
        };
        // Act
        var newBook = await bookService.Create(createBookDto);
        // Assert
        Assert.NotNull(newBook);
        if (newBook is null) return;
        Assert.Equal(createBookDto.Title, newBook.Title);
        Assert.Equal(createBookDto.AuthorId, newBook.AuthorId);
        Assert.Equal(createBookDto.GenreId, newBook.GenreId);
        Assert.Equal(createBookDto.Amount, newBook.Amount);
        Assert.Equal(formerBookRepositoryLength + 1, _expectedBooks.Count);
    }
    
    [Fact]
    public async void GIVEN_incorrect_CreateBookDto_WHEN_Create_method_is_invoked_THEN_null_is_returned()
    {
        // Arrange
        var bookService = GetDefaultBookService();
        var createBookDto = Fixture.Create<CreateBookDTO>();
        var formerBookRepositoryLength = _expectedBooks.Count;
        // Act
        var newBook = await bookService.Create(createBookDto);
        // Assert
        Assert.Null(newBook);
        Assert.Equal(formerBookRepositoryLength, _expectedBooks.Count);
    }

    [Fact]
    public async void GIVEN___WHEN_GetAll_method_is_invoked_THEN_correct_Book_List_is_returned()
    {
        // Arrange
        var bookService = GetDefaultBookService();
        // Act
        var actualBooks = await bookService.GetAll();
        // Assert
        Assert.Equal(_expectedBooks, actualBooks);
    }
    
    [Fact]
    public async void GIVEN_existing_id_WHEN_Get_method_is_invoked_THEN_correct_Book_is_returned()
    {
        // Arrange
        var bookService = GetDefaultBookService();
        var existingId = _expectedBooks[0].Id;
        // Act
        var actualBook = await bookService.Get(existingId);
        // Assert
        Assert.Equal(_expectedBooks[0], actualBook);
    }
    
    [Fact]
    public async void GIVEN_non_existent_id_WHEN_Get_method_is_invoked_THEN_null_is_returned()
    {
        // Arrange
        var bookService = GetDefaultBookService();
        var nonExistentId = Fixture.Create<Book>().Id;
        // Act
        var actualBook = await bookService.Get(nonExistentId);
        // Assert
        Assert.Null(actualBook);
    }

    [Fact]
    public async void GIVEN_existing_UpdateBookDto_WHEN_Update_method_is_invoked_THEN_updated_Book_is_returned()
    {
        // Arrange
        var bookService = GetDefaultBookService();
        var existingUpdateBookDto = new UpdateBookDTO()
            {
                Id = _expectedBooks[0].Id, 
                Title = Fixture.Create<string>(),
                AuthorId = _expectedAuthors[1].Id,
                GenreId = _expectedGenres[2].Id,
                Amount = Fixture.Create<int>()
            };
        // Act
        var actualBook = await bookService.Update(existingUpdateBookDto);
        
        // Assert
        Assert.Equal(_expectedBooks[0], actualBook);
    }
    
    [Fact]
    public async void GIVEN_non_existent_UpdateBookDto_WHEN_Update_method_is_invoked_THEN_null_is_returned()
    {
        // Arrange
        var bookService = GetDefaultBookService();
        var nonExistentBook = Fixture.Create<Book>();
        var nonExistentUpdateBookDto = new UpdateBookDTO()
        {
            Id = nonExistentBook.Id, 
            Title = nonExistentBook.Title,
            AuthorId = nonExistentBook.AuthorId,
            GenreId = nonExistentBook.GenreId,
            Amount = nonExistentBook.Amount
        };
        // Act
        var updatedBook = await bookService.Update(nonExistentUpdateBookDto);
        // Assert
        Assert.Null(updatedBook);
    }
    
    [Fact]
    public async void GIVEN_existing_id_WHEN_Delete_method_is_invoked_THEN_deleted_Book_is_returned()
    {
        // Arrange
        var bookService = GetDefaultBookService();
        var existingBook = _expectedBooks[0];
        var formerBookRepositoryLength = _expectedBooks.Count;
        // Act
        var actualBook = await bookService.Delete(existingBook.Id);
        // Assert
        Assert.Equal(existingBook, actualBook);
        Assert.Equal(formerBookRepositoryLength - 1, _expectedBooks.Count);
    }
    
    [Fact]
    public async void GIVEN_non_existent_id_WHEN_Delete_method_is_invoked_THEN_null_is_returned()
    {
        // Arrange
        var bookService = GetDefaultBookService();
        var nonExistentBook = Fixture.Create<Book>();
        var formerBookRepositoryLength = _expectedBooks.Count;
        // Act
        var actualBook = await bookService.Delete(nonExistentBook.Id);
        // Assert
        Assert.Null(actualBook);
        Assert.Equal(formerBookRepositoryLength, _expectedBooks.Count);
    }
}