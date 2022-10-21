using Library.DTO.Requests;
using Library.Domain;

namespace Library.Services.Authors;

public interface IAuthorService
{
    Task<Author> Create(CreateAuthorDTO request);
    Task<IEnumerable<Author>> GetAll();
    Task<Author?> Get(int id);
    Task<Author?> Update(UpdateAuthorDTO updateAuthorDto);
    Task<Author?> Delete(int id);
}