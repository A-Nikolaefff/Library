using Library.Domain;
using Library.DTO.Requests;
using Library.Repository.Interfaces;
using Library.Repository.Repositories;
using Library.Specifications;

namespace Library.Services.Authors;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;
    
    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<Author> Create(CreateAuthorDTO request)
    {
        var authorData = new Author()
        {
            Name = request.Name
        };
        Author author = await _authorRepository.CreateAsync(authorData);
        await _authorRepository.SaveChangesAsync();
        return author;
    }
    
    public async Task<IEnumerable<Author>> GetAll()
    {
        return await _authorRepository.Get();
    }

    public async Task<Author?> Get(int id)
    {
        return await _authorRepository.GetFirstOrDefault(new ByIdSpec<Author>(id));
    }

    public async Task<Author?> Update(Author authorData)
    {
        var author = await _authorRepository.GetFirstOrDefault(new ByIdSpec<Author>(authorData.Id));
        if (author is null) return author;
        author.Name = authorData.Name;
        _authorRepository.Update(author);
        await _authorRepository.SaveChangesAsync();
        return author;
    }

    public async Task<Author?> Delete(int id)
    {
        Author? author = await _authorRepository.Delete(id);
        if (author is not null)
        {
            await _authorRepository.SaveChangesAsync();
        }
        return author;
    }
}