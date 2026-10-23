
namespace Application.Authors.Dtos
{
    public record AuthorDto(
        int Id,
        int IdBook,
        string FirstName,
        string LastName,
        string FullName,
        int BooksCount);
}
