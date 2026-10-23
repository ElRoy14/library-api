using Application.Authors;
using Application.Books;
using Infrastructure.ExternalApi;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<FakeRestApiOptions>()
                .Bind(configuration.GetSection(FakeRestApiOptions.SectionName))
                .Validate(
                    options => Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out _),
                    "FakeRestApi:La BaseUrl debe ser una URL válida.")
                .ValidateOnStart();

            services.AddHttpClient<IBookRepository, BookRepository>(ConfigureFakeRestApiClient);
            services.AddHttpClient<IAuthorRepository, AuthorRepository>(ConfigureFakeRestApiClient);

            return services;
        }

        private static void ConfigureFakeRestApiClient(IServiceProvider serviceProvider, HttpClient client)
        {
            var options = serviceProvider.GetRequiredService<IOptions<FakeRestApiOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
        }
    }
}
