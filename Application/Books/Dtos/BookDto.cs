using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Books.Dtos
{
    public record BookDto(
        int Id,
        string Title,
        string Description,
        int PageCount,
        string Excerpt,
        DateTime PublishDate);
}
