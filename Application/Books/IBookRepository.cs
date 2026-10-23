using Domain.Entities;
using System;

namespace Application.Books
{
    public interface IBookRepository
    {
        Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Book> CreateAsync(Book book, CancellationToken cancellationToken = default);
        Task<Book> UpdateAsync(int id, Book book, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
