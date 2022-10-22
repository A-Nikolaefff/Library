using Library.Domain;
using Library.DTO.Requests;
using Library.Repository.Interfaces;
using Library.Services.Authors;
using Library.Specifications;
using LibraryTests.MockBuilders;

namespace LibraryTests;

public class AuthorServiceTests
{
    private static readonly Fixture Fixture;

    static AuthorServiceTests()
    {
        Fixture = new Fixture();
        Fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    private readonly List<Author> _expectedAuthors = new List<Author>()
    {
        Fixture.Create<Author>(),
        Fixture.Create<Author>(),
        Fixture.Create<Author>(),
    };
    
    private IAuthorService GetDefaultAuthorService()
    {
        var mock = AuthorRepositoryMockBuilder.Create(_expectedAuthors);
        return new AuthorService(mock.Object);
    }
    
    [Fact]
    public async void GIVEN_CreateAuthorDto_WHEN_Create_method_is_invoked_THEN_correct_Author_is_returned()
    {
        // Arrange
        var authorService = GetDefaultAuthorService();
        var createAuthorDto = Fixture.Create<CreateAuthorDTO>();
        var formerRepositoryLength = _expectedAuthors.Count;
        // Act
        Author newAuthor = await authorService.Create(createAuthorDto);
        // Assert
        Assert.Equal(createAuthorDto.Name, newAuthor.Name);
        Assert.Equal(formerRepositoryLength + 1, _expectedAuthors.Count);
    }

    [Fact]
    public async void GIVEN___WHEN_GetAll_method_is_invoked_THEN_correct_Author_List_is_returned()
    {
        // Arrange
        var authorService = GetDefaultAuthorService();
        // Act
        var actualAuthors = await authorService.GetAll();
        // Assert
        Assert.Equal(_expectedAuthors, actualAuthors);
    }
    
    [Fact]
    public async void GIVEN_existing_id_WHEN_Get_method_is_invoked_THEN_correct_Author_is_returned()
    {
        // Arrange
        var authorService = GetDefaultAuthorService();
        var existingId = _expectedAuthors[0].Id;
        // Act
        var actualAuthor = await authorService.Get(existingId);
        // Assert
        Assert.Equal(_expectedAuthors[0], actualAuthor);
    }
    
    [Fact]
    public async void GIVEN_non_existent_id_WHEN_Get_method_is_invoked_THEN_null_is_returned()
    {
        // Arrange
        var authorService = GetDefaultAuthorService();
        var nonExistentId = Fixture.Create<Author>().Id;
        // Act
        var author = await authorService.Get(nonExistentId);
        // Assert
        Assert.Null(author);
    }

    [Fact]
    public async void GIVEN_existing_UpdateAuthorDto_WHEN_Update_method_is_invoked_THEN_updated_author_is_returned()
    {
        // Arrange
        var authorService = GetDefaultAuthorService();
        var existingUpdateAuthorDto = new UpdateAuthorDTO()
            { Id = _expectedAuthors[0].Id, Name = Fixture.Create<string>()};
        // Act
        var author = await authorService.Update(existingUpdateAuthorDto);
        
        // Assert
        Assert.Equal(_expectedAuthors[0], author);
    }
    
    [Fact]
    public async void GIVEN_non_existent_UpdateAuthorDto_WHEN_Update_method_is_invoked_THEN_null_is_returned()
    {
        // Arrange
        var authorService = GetDefaultAuthorService();
        var nonExistentAuthor = Fixture.Create<Author>();
        var nonExistentUpdateAuthorDto = new UpdateAuthorDTO() 
            { Id = nonExistentAuthor.Id, Name = nonExistentAuthor.Name};
        // Act
        var updatedAuthor = await authorService.Update(nonExistentUpdateAuthorDto);
        // Assert
        Assert.Null(updatedAuthor);
    }
    
    [Fact]
    public async void GIVEN_existing_id_WHEN_Delete_method_is_invoked_THEN_deleted_Author_is_returned()
    {
        // Arrange
        var authorService = GetDefaultAuthorService();
        var existingAuthor = _expectedAuthors[0];
        var formerRepositoryLength = _expectedAuthors.Count;
        // Act
        var actualAuthor = await authorService.Delete(existingAuthor.Id);
        // Assert
        Assert.Equal(existingAuthor, actualAuthor);
        Assert.Equal(formerRepositoryLength - 1, _expectedAuthors.Count);
    }
    
    [Fact]
    public async void GIVEN_non_existent_id_WHEN_Delete_method_is_invoked_THEN_null_is_returned()
    {
        // Arrange
        var authorService = GetDefaultAuthorService();
        var nonExistentId = Fixture.Create<Author>().Id;
        var formerRepositoryLength = _expectedAuthors.Count;
        // Act
        var author = await authorService.Delete(nonExistentId);
        // Assert
        Assert.Null(author);
        Assert.Equal(formerRepositoryLength, _expectedAuthors.Count);
    }
}