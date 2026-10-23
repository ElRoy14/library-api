using Application.Authors.Dtos;
using Application.Authors;
using Application.Common.Exceptions;
using Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Authors
{
    public class AuthorServiceTests
    {
        private readonly IAuthorRepository _authorRepository = Substitute.For<IAuthorRepository>();
        private readonly AuthorService _sut;

        public AuthorServiceTests()
        {
            _sut = new AuthorService(_authorRepository);
        }

        [Fact]
        public async Task GetAllAsync_CountsBooksByAuthorFullName()
        {
            GivenExistingAuthors(
                CreateAuthor(1, 1, "Juan", "Bosch"),
                CreateAuthor(2, 2, "Juan", "Bosch"),
                CreateAuthor(3, 3, "Salomé", "Ureña"));

            var result = await _sut.GetAllAsync();

            Assert.Equal(2, result.Single(author => author.Id == 1).BooksCount);
            Assert.Equal(2, result.Single(author => author.Id == 2).BooksCount);
            Assert.Equal(1, result.Single(author => author.Id == 3).BooksCount);
        }

        [Fact]
        public async Task GetAllAsync_IgnoresCaseWhenGroupingNames()
        {
            GivenExistingAuthors(
                CreateAuthor(1, 1, "Juan", "Bosch"),
                CreateAuthor(2, 2, "JUAN", "bosch"));

            var result = await _sut.GetAllAsync();

            Assert.All(result, author => Assert.Equal(2, author.BooksCount));
        }

        [Fact]
        public async Task GetByIdAsync_CountsTheAuthorItselfOnlyOnce()
        {
            var author = CreateAuthor(1, 1, "Salomé", "Ureña");
            _authorRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(author);
            GivenExistingAuthors(
                author,
                CreateAuthor(2, 2, "Salomé", "Ureña"),
                CreateAuthor(3, 3, "Juan", "Bosch"));

            var result = await _sut.GetByIdAsync(1);

            Assert.Equal(2, result.BooksCount);
        }

        [Fact]
        public async Task CreateAsync_IncludesTheNewAuthorInTheCount()
        {
            GivenExistingAuthors(CreateAuthor(1, 1, "Pedro", "Mir"));
            _authorRepository.CreateAsync(Arg.Any<Author>(), Arg.Any<CancellationToken>())
                .Returns(call => call.Arg<Author>());
            var request = new AuthorRequest { IdBook = 2, FirstName = "Pedro", LastName = "Mir" };

            var result = await _sut.CreateAsync(request);

            Assert.Equal(2, result.BooksCount);
        }

        [Fact]
        public async Task UpdateAsync_DoesNotCountThePreviousVersionOfTheAuthor()
        {
            _authorRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
                .Returns(CreateAuthor(1, 1, "Pedro", "Mir"));
            GivenExistingAuthors(
                CreateAuthor(1, 1, "Pedro", "Mir"),
                CreateAuthor(2, 2, "Juan", "Bosch"));
            _authorRepository.UpdateAsync(1, Arg.Any<Author>(), Arg.Any<CancellationToken>())
                .Returns(call => call.Arg<Author>());
            var request = new AuthorRequest { IdBook = 1, FirstName = "Juan", LastName = "Bosch" };

            var result = await _sut.UpdateAsync(1, request);

            Assert.Equal("Juan Bosch", result.FullName);
            Assert.Equal(2, result.BooksCount);
        }

        [Fact]
        public async Task DeleteAsync_WhenAuthorDoesNotExist_ThrowsWithoutCallingDelete()
        {
            _authorRepository.GetByIdAsync(99, Arg.Any<CancellationToken>()).Returns((Author?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _sut.DeleteAsync(99));
            await _authorRepository.DidNotReceiveWithAnyArgs().DeleteAsync(default, default);
        }

        private void GivenExistingAuthors(params Author[] authors) =>
            _authorRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(authors.ToList());

        private static Author CreateAuthor(int id, int idBook, string firstName, string lastName) => new()
        {
            Id = id,
            IdBook = idBook,
            FirstName = firstName,
            LastName = lastName
        };
    }
}
