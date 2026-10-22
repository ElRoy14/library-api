using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authors.Dtos
{
    public record AuthorDto(
        int Id,
        int IdBook,
        string FirstName,
        string LastName,
        string FullName,
        int BooksCount);
}
