namespace Library.DTO.Requests;

public class CreateBookDTO
{
    public string Title { get; set; }
    
    public int AuthorId { get; set; }

    public int GenreId { get; set; }

    public int Amount { get; set; }
}