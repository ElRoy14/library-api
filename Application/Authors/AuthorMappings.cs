using Application.Authors.Dtos;
using Domain.Entities;

namespace Application.Authors
{
    internal static class AuthorMappings
    {
        public static AuthorDto ToDto(this Author author, int booksCount) =>
            new(author.Id, author.IdBook, author.FirstName, author.LastName, author.FullName, booksCount);

        public static Author ToEntity(this AuthorRequest request, int id = 0) =>
            new()
            {
                Id = id,
                IdBook = request.IdBook,
                FirstName = request.FirstName,
                LastName = request.LastName
            };
    }
}
