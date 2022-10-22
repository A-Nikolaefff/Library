using Library.Domain;
using Library.Repository.Interfaces;
using Library.Specifications;

namespace LibraryTests.MockBuilders;

public static class AuthorRepositoryMockBuilder
{
    public static Mock<IAuthorRepository> Create(List<Author> expectedAuthors)
    {
        var mock = new Mock<IAuthorRepository>();
        
        mock.Setup(ar => ar.CreateAsync(It.IsAny<Author>()))
            .ReturnsAsync((Author authorData) =>
            {
                var author = new Author() {Name = authorData.Name, Id = new Random().Next()};
                expectedAuthors.Add(author);
                return author;
            });
        
        mock.Setup(ar => ar.Get()).ReturnsAsync(expectedAuthors);

        mock.Setup(ar => ar.GetFirstOrDefault(It.IsAny<ByIdSpec<Author>>()))
            .ReturnsAsync(
                (ByIdSpec<Author> spec) => expectedAuthors.FirstOrDefault(a => a.Id == spec.Id));
        
        mock.Setup(ar => ar.Update(It.IsAny<Author>()));

        mock.Setup(ar => ar.Delete(It.IsAny<int>())).ReturnsAsync(
            (int id) =>
            {
                var author = expectedAuthors.FirstOrDefault(a => a.Id == id);
                if (author is null) return null;
                expectedAuthors.Remove(author);
                return author;
            });

        return mock;
    }
}