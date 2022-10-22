using Library.Domain;
using Library.Repository.Interfaces;
using Library.Specifications;

namespace LibraryTests.MockBuilders;

public static class GenreRepositoryMockBuilder
{
    public static Mock<IGenreRepository> Create(List<Genre> expectedGenres)
    {
        var mock = new Mock<IGenreRepository>();
        
        mock.Setup(ar => ar.CreateAsync(It.IsAny<Genre>()))
            .ReturnsAsync((Genre genreData) =>
            {
                var genre = new Genre() {Name = genreData.Name, Id = new Random().Next()};
                expectedGenres.Add(genre);
                return genre;
            });
        
        mock.Setup(ar => ar.Get()).ReturnsAsync(expectedGenres);

        mock.Setup(ar => ar.GetFirstOrDefault(It.IsAny<ByIdSpec<Genre>>()))
            .ReturnsAsync(
                (ByIdSpec<Genre> spec) => expectedGenres.FirstOrDefault(a => a.Id == spec.Id));
        
        mock.Setup(ar => ar.Update(It.IsAny<Genre>()));

        mock.Setup(ar => ar.Delete(It.IsAny<int>())).ReturnsAsync(
            (int id) =>
            {
                var genre = expectedGenres.FirstOrDefault(a => a.Id == id);
                if (genre is null) return null;
                expectedGenres.Remove(genre);
                return genre;
            });

        return mock;
    }
}