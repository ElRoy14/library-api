using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExternalApi
{
    public class FakeRestApiOptions
    {
        public const string SectionName = "FakeRestApi";

        public string BaseUrl { get; init; } = string.Empty;
    }
}
