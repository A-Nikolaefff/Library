namespace Library.DTO.Requests;

public class UpdateBookDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    
    public int AuthorId { get; set; }
    
    public int GenreId { get; set; }
    
    public int Amount { get; set; }
}