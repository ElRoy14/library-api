using Application.Authors;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private const string Resource = "Authors";
        private readonly HttpClient _httpClient;

        public AuthorRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyList<Author>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var authors = await _httpClient.GetFromJsonAsync<List<Author>>(Resource, cancellationToken);
            return authors ?? [];
        }

        public async Task<Author?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.GetAsync($"{Resource}/{id}", cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Author>(cancellationToken);
        }

        public async Task<IReadOnlyList<Author>> GetByBookIdAsync(int bookId, CancellationToken cancellationToken = default)
        {
            var authors = await _httpClient.GetFromJsonAsync<List<Author>>(
                $"{Resource}/authors/books/{bookId}", cancellationToken);

            return authors ?? [];
        }

        public async Task<Author> CreateAsync(Author author, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.PostAsJsonAsync(Resource, author, cancellationToken);
            return await ReadAuthorAsync(response, cancellationToken);
        }

        public async Task<Author> UpdateAsync(int id, Author author, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.PutAsJsonAsync($"{Resource}/{id}", author, cancellationToken);
            return await ReadAuthorAsync(response, cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.DeleteAsync($"{Resource}/{id}", cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        private static async Task<Author> ReadAuthorAsync(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Author>(cancellationToken)
                ?? throw new InvalidOperationException("La API externa retornó una respuesta de autor vacía.");
        }
    }
}
