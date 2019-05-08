using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace FakeApi
{
    internal class HttpResponseMock
    {
        private CookieCollection _cookieCollection;
        private WebHeaderCollection _headerCollection;
        private int _currentIndexFile;

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
        public IEnumerable<string> Files { get; set; }
        public string WebExceptionMessage { get; set; }
        public ApiException CustomApiException { get; set; }
        public bool HasCustomException => CustomApiException != null;
        public bool HasWebException => !string.IsNullOrEmpty(WebExceptionMessage);
        public bool HasFile => !string.IsNullOrEmpty(File);
        public bool HasFiles => Files != null && Files.Any();
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

        public string GetNextFile()
        {
            if(!HasFiles)
            {
                throw new InvalidOperationException();
            }

            if(_currentIndexFile == Files.Count())
            {
                _currentIndexFile = 0;
            }

            var file = Files.ElementAt(_currentIndexFile);
            _currentIndexFile++;
            return file;
        }
    }
}
