using Application.Books.Dtos;

namespace Application.Books
{
    public interface IBookService
    {
        Task<IReadOnlyList<BookDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<BookDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<BookDetailDto> GetDetailAsync(int id, CancellationToken cancellationToken = default);
        Task<BookDto> CreateAsync(BookRequest request, CancellationToken cancellationToken = default);
        Task<BookDto> UpdateAsync(int id, BookRequest request, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
