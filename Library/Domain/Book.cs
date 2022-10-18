using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Domain;

[Table("book")] 
public class Book : Entity
{
    [Column("title")]
    public string Title { get; set; }
    
    [Column("author_id")]
    public int AuthorId { get; set; }

    public Author Author { get; set; }
    
    [Column("genre_id")]
    public int GenreId { get; set; }

    public Genre Genre { get; set; }

    [Column("amount")]
    public int Amount { get; set; }
}