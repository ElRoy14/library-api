using Application.Authors;
using Application.Books;
using Application.Books.Dtos;
using Application.Common.Exceptions;
using Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Books
{
    public class BookServiceTests
    {
        private readonly IBookRepository _bookRepository = Substitute.For<IBookRepository>();
        private readonly IAuthorRepository _authorRepository = Substitute.For<IAuthorRepository>();
        private readonly BookService _sut;

        public BookServiceTests()
        {
            _sut = new BookService(_bookRepository, _authorRepository);
        }

        [Fact]
        public async Task GetByIdAsync_WhenBookExists_ReturnsMappedDto()
        {
            var book = CreateBook(1);
            _bookRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(book);

            var result = await _sut.GetByIdAsync(1);

            var expected = new BookDto(book.Id, book.Title, book.Description, book.PageCount, book.Excerpt, book.PublishDate);
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetByIdAsync_WhenBookDoesNotExist_ThrowsNotFoundException()
        {
            _bookRepository.GetByIdAsync(99, Arg.Any<CancellationToken>()).Returns((Book?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _sut.GetByIdAsync(99));
        }

        [Fact]
        public async Task GetDetailAsync_WhenBookExists_ReturnsBookWithItsAuthors()
        {
            _bookRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(CreateBook(1));
            _authorRepository.GetByBookIdAsync(1, Arg.Any<CancellationToken>()).Returns(new List<Author>
        {
            CreateAuthor(10, 1, "Jane", "Doe"),
            CreateAuthor(11, 1, "John", "Smith")
        });

            var result = await _sut.GetDetailAsync(1);

            Assert.Equal(1, result.Id);
            Assert.Equal(2, result.Authors.Count);
            Assert.Contains(result.Authors, author => author.FullName == "Jane Doe");
            Assert.Contains(result.Authors, author => author.FullName == "John Smith");
        }

        [Fact]
        public async Task CreateAsync_MapsRequestToEntityBeforeSaving()
        {
            var request = new BookRequest
            {
                Title = "Moby Dick",
                Description = "Una aventura inigualable en el mar.",
                PageCount = 464,
                Excerpt = "Ahora el Señor había preparado un gran pez para tragarse a Jonás.",
                PublishDate = new DateTime(2008, 8, 1)
            };
            _bookRepository.CreateAsync(Arg.Any<Book>(), Arg.Any<CancellationToken>())
                .Returns(call => call.Arg<Book>());

            var result = await _sut.CreateAsync(request);

            await _bookRepository.Received(1).CreateAsync(
                Arg.Is<Book>(book =>
                    book.Title == request.Title &&
                    book.PageCount == request.PageCount &&
                    book.PublishDate == request.PublishDate),
                Arg.Any<CancellationToken>());
            Assert.Equal(request.Title, result.Title);
        }

        [Fact]
        public async Task UpdateAsync_WhenBookDoesNotExist_ThrowsWithoutCallingUpdate()
        {
            _bookRepository.GetByIdAsync(99, Arg.Any<CancellationToken>()).Returns((Book?)null);
            var request = new BookRequest { Title = "El diario de Anna Frank", PageCount = 1, PublishDate = DateTime.Today };

            await Assert.ThrowsAsync<NotFoundException>(() => _sut.UpdateAsync(99, request));
            await _bookRepository.DidNotReceiveWithAnyArgs().UpdateAsync(default, default!, default);
        }

        [Fact]
        public async Task DeleteAsync_WhenBookDoesNotExist_ThrowsWithoutCallingDelete()
        {
            _bookRepository.GetByIdAsync(99, Arg.Any<CancellationToken>()).Returns((Book?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _sut.DeleteAsync(99));
            await _bookRepository.DidNotReceiveWithAnyArgs().DeleteAsync(default, default);
        }

        private static Book CreateBook(int id) => new()
        {
            Id = id,
            Title = $"Catalepsia {id}",
            Description = "Descripción",
            PageCount = 100,
            Excerpt = "Extracto",
            PublishDate = new DateTime(2020, 1, 1)
        };

        private static Author CreateAuthor(int id, int idBook, string firstName, string lastName) => new()
        {
            Id = id,
            IdBook = idBook,
            FirstName = firstName,
            LastName = lastName
        };
    }
}
