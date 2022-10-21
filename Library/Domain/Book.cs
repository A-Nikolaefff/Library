using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Domain;

[Table("book")] 
public class Book : Entity
{
    [Column("title")] 
    public string Title { get; set; } = null!;
    
    [Column("author_id")]
    public int AuthorId { get; set; }

    public Author Author { get; set; } = null!;
    
    [Column("genre_id")]
    public int GenreId { get; set; }

    public Genre Genre { get; set; } = null!;

    [Column("amount")]
    public int Amount { get; set; }

    public Book(int authorId, int genreId)
    {
        AuthorId = authorId;
        GenreId = genreId;
    }

    public Book(string title, Author author, Genre genre, int amount)
    {
        Title = title;
        Author = author;
        Genre = genre;
        Amount = amount;
    }
}