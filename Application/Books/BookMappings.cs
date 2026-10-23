using Application.Books.Dtos;
using Domain.Entities;

namespace Application.Books
{
    internal static class BookMappings
    {
        public static BookDto ToDto(this Book book) =>
            new(book.Id, book.Title, book.Description, book.PageCount, book.Excerpt, book.PublishDate);

        public static BookDetailDto ToDetailDto(this Book book, IEnumerable<Author> authors) =>
            new(
                book.Id,
                book.Title,
                book.Description,
                book.PageCount,
                book.Excerpt,
                book.PublishDate,
                authors.Select(a => new BookAuthorDto(a.Id, a.FullName)).ToList());

        public static Book ToEntity(this BookRequest request, int id = 0) =>
            new()
            {
                Id = id,
                Title = request.Title,
                Description = request.Description,
                PageCount = request.PageCount,
                Excerpt = request.Excerpt,
                PublishDate = request.PublishDate!.Value
            };
    }
}
