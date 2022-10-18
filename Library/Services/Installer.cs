using Library.Repository.Interfaces;
using Library.Repository.Repositories;
using Library.Services.Authors;
using Library.Services.Books;
using Library.Services.Genres;

namespace Library.Services;

public static class Installer
{
    public static void AddServices(this IServiceCollection container)
    {
        container
            .AddScoped<IBookRepository, BookRepository>()
            .AddScoped<IAuthorRepository, AuthorRepository>()
            .AddScoped<IGenreRepository, GenreRepository>()
            
            .AddScoped<IBookService, BookService>()
            .AddScoped<IAuthorService, AuthorService>()
            .AddScoped<IGenreService, GenreService>();
    }
}