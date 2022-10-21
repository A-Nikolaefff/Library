using Library.Domain;
using Library.DTO.Requests;

namespace Library.Services.Books;

public interface IBookService
{
    Task<Book?> Create(CreateBookDTO bookDto);
    Task<IEnumerable<Book>> GetAll();
    Task<Book?> Get(int id);
    Task<Book?> Update(Book bookData);
    Task<Book?> Delete(int id);
}