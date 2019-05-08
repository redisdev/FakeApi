using System.Collections.Generic;
using System;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace FakeApi
{
    internal class Config
    {
        private CookieCollection _cookieCollection;
        private WebHeaderCollection _headerCollection;

        public int? DefaultDelay { get; set; }

        public int DefaultHttpCode { get; set; }

        public long DefaultContentLength { get; set; }

        public string DefaultContentType { get; set; }

        public bool DefaultIsFromCache { get; set; }

        public bool DefaultIsMutuallyAuthenticated { get; set; }

        public string DefaultMethod { get; set; }

        public string DefaultResponseUri { get; set; }

        public string DefaultResponse { get; set; }

        public string DefaultStatusDescription { get; set; }

        public IEnumerable<Cookie> DefaultCookies { get; set; }

        public IEnumerable<HttpHeader> DefaultHeaders { get; set; }

        public IEnumerable<ApiConfig> Apis { get; set; }

        public IEnumerable<string> ApisDirectories { get; set; }

        [JsonIgnore]
        public CookieCollection CookieCollection
        {
            get
            {
                if(DefaultCookies == null)
                {
                    return null;
                }

                if(_cookieCollection  == null)
                {
                    _cookieCollection = new CookieCollection();
                    foreach (var cookie in DefaultCookies)
                    {
                        _cookieCollection.Add(cookie);
                    }
                }

                return _cookieCollection;
            }
        }

        [JsonIgnore]
        public WebHeaderCollection DefaultHeaderCollection
        {
            get
            {
                if (DefaultHeaders == null)
                {
                    return null;
                }

                if (_headerCollection == null)
                {
                    _headerCollection = new WebHeaderCollection();
                    foreach (var header in DefaultHeaders)
                    {
                        _headerCollection.Add(header.Name, header.Value);
                    }
                }

                return _headerCollection;
            }
        }

        public void Validate()
        {
            if(Apis == null || !Apis.Any())
            {
                throw new Exception("No apis configured");
            }

            if (Apis.Any(a => string.IsNullOrEmpty(a.Url)))
            {
                throw new Exception("At least one api has no url configured");
            }

            var responses = Apis.Where(a => a.Responses != null).SelectMany(a => a.Responses).ToList();

            if (string.IsNullOrEmpty(DefaultMethod)
                && (!responses.Any() || responses.Any(r => string.IsNullOrEmpty(r.Method))))
            {
                throw new Exception("At least one api has no http method configured");
            }

            if(string.IsNullOrEmpty(DefaultResponse)
                && (!responses.Any() 
                    || responses.Any(r => !r.HasFile && !r.HasFiles && !r.HasWebException && !r.HasCustomException && string.IsNullOrEmpty(r.Response))))
            {
                throw new Exception("At least one api has no response configured");
            }
        }

        public HttpResponseMock GetResponseMock(string url, string method)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (string.IsNullOrEmpty(method))
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (Apis == null)
            {
                throw new InvalidOperationException($"Try to get fake response for url {url} but apis config not loaded");
            }

            var apiCfg = Apis.Where(a =>                     TemplateMatcher.Match(new Uri(a.Url), new Uri(url))                     && a.Responses != null)                 .SelectMany(a => a.Responses)                 .SingleOrDefault(a => a.Active                                 && (string.Equals(a.Method ?? DefaultMethod, method, StringComparison.InvariantCultureIgnoreCase)));

            if (apiCfg == null)
            {
                return new HttpResponseMock();
            }

            return apiCfg;
        }
    }
}
