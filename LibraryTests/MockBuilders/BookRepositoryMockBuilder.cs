using System.Linq.Expressions;
using Library.Domain;
using Library.Repository.Interfaces;
using Library.Services.Books;
using Library.Specifications;
using Moq;

namespace LibraryTests.MockBuilders;

public static class BookRepositoryMockBuilder
{
    public static Mock<IBookRepository> Create(List<Book> expectedBooks)
    {
        /* In the BookRepository mock object, there is no need to check if books have existing authors and genres,
         because this is a function of BookService. It is assumed that the passed book or ID exists. */
        var mock = new Mock<IBookRepository>();

        mock.Setup(ar => ar.CreateAsync(It.IsAny<Book>()))
            .ReturnsAsync((Book bookData) =>
            {
                var book = new Book(bookData.Author.Id, bookData.Genre.Id)
                {
                    Id = new Random().Next(),
                    Title = bookData.Title,
                    Amount = bookData.Amount
                };
                expectedBooks.Add(book);
                return book;
            });

        mock.Setup(ar => ar.Get(b => b.Author, b => b.Genre))
            .ReturnsAsync(expectedBooks);

        mock.Setup(ar => ar.GetFirstOrDefault(It.IsAny<ByIdSpec<Book>>()))
            .ReturnsAsync((ByIdSpec<Book> spec) => expectedBooks.FirstOrDefault(b => b.Id == spec.Id));
        
        mock.Setup(ar => ar.GetFirstOrDefault(It.IsAny<ByIdSpec<Book>>(), 
                b => b.Author, b => b.Genre))
            .ReturnsAsync((ByIdSpec<Book> spec, Expression<Func<Book, object>>[] includes) 
                    => expectedBooks.FirstOrDefault(b => b.Id == spec.Id));
        
        mock.Setup(ar => ar.Update(It.IsAny<Book>()));

        mock.Setup(ar => ar.Delete(It.IsAny<int>())).ReturnsAsync(
            (int id) => 
            {
                var book = expectedBooks.FirstOrDefault(a => a.Id == id);
                if (book is null) return null;
                expectedBooks.Remove(book);
                return book;
            });

        return mock;
    }
}