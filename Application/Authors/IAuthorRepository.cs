using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authors
{
    public interface IAuthorRepository
    {
        Task<IReadOnlyList<Author>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Author?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Author>> GetByBookIdAsync(int bookId, CancellationToken cancellationToken = default);
        Task<Author> CreateAsync(Author author, CancellationToken cancellationToken = default);
        Task<Author> UpdateAsync(int id, Author author, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
