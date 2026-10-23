using Application.Books;
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
    public class BookRepository : IBookRepository
    {
        private const string Resource = "Books";
        private readonly HttpClient _httpClient;

        public BookRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var books = await _httpClient.GetFromJsonAsync<List<Book>>(Resource, cancellationToken);
            return books ?? [];
        }

        public async Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.GetAsync($"{Resource}/{id}", cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Book>(cancellationToken);
        }

        public async Task<Book> CreateAsync(Book book, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.PostAsJsonAsync(Resource, book, cancellationToken);
            return await ReadBookAsync(response, cancellationToken);
        }

        public async Task<Book> UpdateAsync(int id, Book book, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.PutAsJsonAsync($"{Resource}/{id}", book, cancellationToken);
            return await ReadBookAsync(response, cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.DeleteAsync($"{Resource}/{id}", cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        private static async Task<Book> ReadBookAsync(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Book>(cancellationToken)
                ?? throw new InvalidOperationException("La API externa retornó una respuesta de un libro vacía.");
        }
    }
}
