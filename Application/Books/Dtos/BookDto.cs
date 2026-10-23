
namespace Application.Books.Dtos
{
    public record BookDto(
        int Id,
        string Title,
        string Description,
        int PageCount,
        string Excerpt,
        DateTime PublishDate);
}
