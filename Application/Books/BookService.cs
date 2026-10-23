using Application.Authors;
using Application.Books.Dtos;
using Application.Common.Exceptions;
using Domain.Entities;

namespace Application.Books
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;

        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        public async Task<IReadOnlyList<BookDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var books = await _bookRepository.GetAllAsync(cancellationToken);
            return books.Select(book => book.ToDto()).ToList();
        }

        public async Task<BookDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var book = await GetExistingBookAsync(id, cancellationToken);
            return book.ToDto();
        }

        public async Task<BookDetailDto> GetDetailAsync(int id, CancellationToken cancellationToken = default)
        {
            var book = await GetExistingBookAsync(id, cancellationToken);
            var authors = await _authorRepository.GetByBookIdAsync(id, cancellationToken);
            return book.ToDetailDto(authors);
        }

        public async Task<BookDto> CreateAsync(BookRequest request, CancellationToken cancellationToken = default)
        {
            var created = await _bookRepository.CreateAsync(request.ToEntity(), cancellationToken);
            return created.ToDto();
        }

        public async Task<BookDto> UpdateAsync(int id, BookRequest request, CancellationToken cancellationToken = default)
        {
            await GetExistingBookAsync(id, cancellationToken);
            var updated = await _bookRepository.UpdateAsync(id, request.ToEntity(id), cancellationToken);
            return updated.ToDto();
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await GetExistingBookAsync(id, cancellationToken);
            await _bookRepository.DeleteAsync(id, cancellationToken);
        }

        private async Task<Book> GetExistingBookAsync(int id, CancellationToken cancellationToken) =>
            await _bookRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException(nameof(Book), id);
    }
}
