
namespace Application.Books.Dtos
{
    public record BookDetailDto(
        int Id,
        string Title,
        string Description,
        int PageCount,
        string Excerpt,
        DateTime PublishDate,
        IReadOnlyList<BookAuthorDto> Authors);
}
