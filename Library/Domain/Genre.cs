using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Domain;

[Table("genre")]
public class Genre : Entity
{
    [Column("name")]
    public string Name { get; set; }
    
    public List<Book> Books { get; set; } = new();
}