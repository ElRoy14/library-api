using Application.Authors.Dtos;
using Application.Books;
using Application.Common.Exceptions;
using Domain.Entities;

namespace Application.Authors
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<IReadOnlyList<AuthorDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var authors = await _authorRepository.GetAllAsync(cancellationToken);

            var booksCountByName = authors
                .GroupBy(author => author.FullName, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(group => group.Key, group => group.Count(), StringComparer.OrdinalIgnoreCase);

            return authors
                .Select(author => author.ToDto(booksCountByName[author.FullName]))
                .ToList();
        }

        public async Task<AuthorDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var author = await GetExistingAuthorAsync(id, cancellationToken);
            var booksCount = await CountBooksAsync(author, cancellationToken);
            return author.ToDto(booksCount);
        }

        public async Task<AuthorDto> CreateAsync(AuthorRequest request, CancellationToken cancellationToken = default)
        {
            var created = await _authorRepository.CreateAsync(request.ToEntity(), cancellationToken);
            var booksCount = await CountBooksAsync(created, cancellationToken);
            return created.ToDto(booksCount);
        }

        public async Task<AuthorDto> UpdateAsync(int id, AuthorRequest request, CancellationToken cancellationToken = default)
        {
            await GetExistingAuthorAsync(id, cancellationToken);
            var updated = await _authorRepository.UpdateAsync(id, request.ToEntity(id), cancellationToken);
            var booksCount = await CountBooksAsync(updated, cancellationToken);
            return updated.ToDto(booksCount);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await GetExistingAuthorAsync(id, cancellationToken);
            await _authorRepository.DeleteAsync(id, cancellationToken);
        }

        private async Task<Author> GetExistingAuthorAsync(int id, CancellationToken cancellationToken) =>
            await _authorRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException(nameof(Author), id);

        private async Task<int> CountBooksAsync(Author author, CancellationToken cancellationToken)
        {
            var authors = await _authorRepository.GetAllAsync(cancellationToken);

            var otherRecords = authors.Count(existing =>
                existing.Id != author.Id &&
                string.Equals(existing.FullName, author.FullName, StringComparison.OrdinalIgnoreCase));

            return otherRecords + 1;
        }
    }
}
