using Library.Domain;
using Library.DTO.Requests;
using Library.Repository.Interfaces;
using Library.Services.Genres;
using Library.Specifications;
using LibraryTests.MockBuilders;

namespace LibraryTests;

public class GenreServiceTests
{
    private static readonly Fixture Fixture;

    static GenreServiceTests()
    {
        Fixture = new Fixture();
        Fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    private readonly List<Genre> _expectedGenres = new List<Genre>()
    {
        Fixture.Create<Genre>(),
        Fixture.Create<Genre>(),
        Fixture.Create<Genre>(),
    };
    
    private IGenreService GetDefaultGenreService()
    {
        var mock = GenreRepositoryMockBuilder.Create(_expectedGenres);
        return new GenreService(mock.Object);
    }
    
    [Fact]
    public async void GIVEN_CreateGenreDto_WHEN_Create_method_is_invoked_THEN_correct_genre_is_returned()
    {
        // Arrange
        var genreService = GetDefaultGenreService();
        var createGenreDto = Fixture.Create<CreateGenreDTO>();
        var formerRepositoryLength = _expectedGenres.Count;
        // Act
        var newGenre = await genreService.Create(createGenreDto);
        // Assert
        Assert.Equal(createGenreDto.Name, newGenre.Name);
        Assert.Equal(formerRepositoryLength + 1, _expectedGenres.Count);
    }

    [Fact]
    public async void GIVEN___WHEN_GetAll_method_is_invoked_THEN_correct_Genre_List_is_returned()
    {
        // Arrange
        var genreService = GetDefaultGenreService();
        // Act
        var actualGenres = await genreService.GetAll();
        // Assert
        Assert.Equal(_expectedGenres, actualGenres);
    }
    
    [Fact]
    public async void GIVEN_existing_id_WHEN_Get_method_is_invoked_THEN_correct_Genre_is_returned()
    {
        // Arrange
        var genreService = GetDefaultGenreService();
        var existingId = _expectedGenres[0].Id;
        // Act
        var actualGenre = await genreService.Get(existingId);
        // Assert
        Assert.Equal(_expectedGenres[0], actualGenre);
    }
    
    [Fact]
    public async void GIVEN_non_existent_id_WHEN_Get_method_is_invoked_THEN_null_is_returned()
    {
        // Arrange
        var genreService = GetDefaultGenreService();
        var nonExistentId = Fixture.Create<Genre>().Id;
        // Act
        var genre = await genreService.Get(nonExistentId);
        // Assert
        Assert.Null(genre);
    }

    [Fact]
    public async void GIVEN_existing_UpdateGenreDto_WHEN_Update_method_is_invoked_THEN_updated_Genre_is_returned()
    {
        // Arrange
        var genreService = GetDefaultGenreService();
        var existingUpdateGenreDto = new UpdateGenreDTO()
            { Id = _expectedGenres[0].Id, Name = Fixture.Create<string>()};
        // Act
        var genre = await genreService.Update(existingUpdateGenreDto);
        
        // Assert
        Assert.Equal(_expectedGenres[0], genre);
    }
    
    [Fact]
    public async void GIVEN_non_existent_UpdateGenreDto_WHEN_Update_method_is_invoked_THEN_null_is_returned()
    {
        // Arrange
        var genreService = GetDefaultGenreService();
        var nonExistentGenre = Fixture.Create<Genre>();
        var nonExistentUpdateGenreDto = new UpdateGenreDTO() 
            { Id = nonExistentGenre.Id, Name = nonExistentGenre.Name};
        // Act
        var updatedGenre = await genreService.Update(nonExistentUpdateGenreDto);
        // Assert
        Assert.Null(updatedGenre);
    }
    
    [Fact]
    public async void GIVEN_existing_id_WHEN_Delete_method_is_invoked_THEN_deleted_Genre_is_returned()
    {
        // Arrange
        var genreService = GetDefaultGenreService();
        var existingGenre = _expectedGenres[0];
        var formerRepositoryLength = _expectedGenres.Count;
        // Act
        var actualGenre = await genreService.Delete(existingGenre.Id);
        // Assert
        Assert.Equal(existingGenre, actualGenre);
        Assert.Equal(formerRepositoryLength - 1, _expectedGenres.Count);
    }
    
    [Fact]
    public async void GIVEN_non_existent_id_WHEN_Delete_method_is_invoked_THEN_null_is_returned()
    {
        // Arrange
        var genreService = GetDefaultGenreService();
        var nonExistentId = Fixture.Create<Genre>().Id;
        var formerRepositoryLength = _expectedGenres.Count;
        // Act
        var genre = await genreService.Delete(nonExistentId);
        // Assert
        Assert.Null(genre);
        Assert.Equal(formerRepositoryLength, _expectedGenres.Count);
    }
}