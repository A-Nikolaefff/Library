using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Domain;

public abstract class Entity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
}