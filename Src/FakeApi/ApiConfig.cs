using System.Collections.Generic;

namespace FakeApi
{
    internal class ApiConfig
    {
        public string Url { get; set; }

        public IEnumerable<HttpResponseMock> Responses { get; set; }
    }
}
