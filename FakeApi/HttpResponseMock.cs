using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace FakeApi
{
    internal class HttpResponseMock
    {
        private CookieCollection _cookieCollection;
        private WebHeaderCollection _headerCollection;

        public int? Delay { get; set; }
        public bool Active { get; set; }
        public int? HttpCode { get; set; }
        public long? ContentLength { get; set; }
        public string ContentType { get; set; }
        public bool? IsFromCache { get; set; }
        public bool? IsMutuallyAuthenticated { get; set; }
        public string Method { get; set; }
        public string ResponseUri { get; set; }
        public string Response { get; set; }
        public string StatusDescription { get; set; }
        public string File { get; set; }
        public string WebExceptionMessage { get; set; }
        public ApiException CustomApiException { get; set; }
        public bool HasCustomException => CustomApiException != null;
        public bool HasWebException => !string.IsNullOrEmpty(WebExceptionMessage);
        public bool HasFile => !string.IsNullOrEmpty(File);
        public IEnumerable<Cookie> Cookies { get; set; }
        public IEnumerable<HttpHeader> Headers { get; set; }

        [JsonIgnore]
        public CookieCollection CookieCollection
        {
            get
            {
                if(Cookies == null)
                {
                    return null;
                }

                if (_cookieCollection == null)
                {
                    _cookieCollection = new CookieCollection();
                    foreach (var cookie in Cookies)
                    {
                        _cookieCollection.Add(cookie);
                    }
                }

                return _cookieCollection;
            }
        }

        [JsonIgnore]
        public WebHeaderCollection HeaderCollection
        {
            get
            {
                if (Headers == null)
                {
                    return null;
                }

                if (_headerCollection == null)
                {
                    _headerCollection = new WebHeaderCollection();
                    foreach (var header in Headers)
                    {
                        _headerCollection.Add(header.Name, header.Value);
                    }
                }

                return _headerCollection;
            }
        }
    }
}
