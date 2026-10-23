using Application.Authors.Dtos;

namespace Application.Authors
{
    public interface IAuthorService
    {
        Task<IReadOnlyList<AuthorDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<AuthorDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<AuthorDto> CreateAsync(AuthorRequest request, CancellationToken cancellationToken = default);
        Task<AuthorDto> UpdateAsync(int id, AuthorRequest request, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
