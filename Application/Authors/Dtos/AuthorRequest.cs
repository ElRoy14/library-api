using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authors.Dtos
{
    public record AuthorRequest
    {
        [Range(1, int.MaxValue)]
        public int IdBook { get; init; }

        [Required, MaxLength(100)]
        public string FirstName { get; init; } = string.Empty;

        [Required, MaxLength(100)]
        public string LastName { get; init; } = string.Empty;
    }
}
