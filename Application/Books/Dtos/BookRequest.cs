using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Books.Dtos
{
    public record BookRequest
    {
        [Required, MaxLength(200)]
        public string Title { get; init; } = string.Empty;

        [MaxLength(2000)]
        public string Description { get; init; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int PageCount { get; init; }

        [MaxLength(2000)]
        public string Excerpt { get; init; } = string.Empty;

        [Required]
        public DateTime? PublishDate { get; init; }
    }
}
