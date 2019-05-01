using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace FakeApi
{
    /// <summary>
    /// Fake http requester.
    /// </summary>
    public class FakeHttpRequester: IHttpRequester
    {
        readonly string _configSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FakeApi.FakeHttpRequester"/> class.
        /// </summary>
        /// <param name="configSource">Apis configuration file path</param>
        public FakeHttpRequester(string configSource)
        {
            _configSource = configSource;
        }

        /// <summary>
        /// Return mock of HttpWebResponse from the information defined in the configuration file
        /// </summary>
        /// <param name="request">Request to be sent</param>
        public HttpWebResponse GetResponse(HttpWebRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var config = ConfigIO.GetConfig(_configSource);
            var apiResponse = config.GetResponseMock(request.RequestUri.AbsoluteUri);

            if (apiResponse.HasCustomException)
            {
                var type = Type.GetType(apiResponse.CustomApiException.FullTypeName);
                throw (Exception)Activator.CreateInstance(type, apiResponse.CustomApiException.ConstructorArgs);
            }

            if (apiResponse.HasWebException)
            {
                throw new WebException(apiResponse.WebExceptionMessage);
            }

            var response = CreateResponseMock(config, apiResponse);

            Task.Delay(apiResponse.Delay ?? config.DefaultDelay).Wait();

            return response.Object;
        }

        /// <summary>
        /// Return asynchronous mock of HttpWebResponse from the information defined in the configuration file
        /// </summary>
        /// <param name="request">Request to be sent</param>
        public async Task<HttpWebResponse> GetResponseAsync(HttpWebRequest request)
        {
            return await Task.FromResult(GetResponse(request));
        }

        private static Mock<HttpWebResponse> CreateResponseMock(Config config, HttpResponseMock apiResponse)
        {
            var responseStream = CreateResponseStream(config, apiResponse);

            var response = new Mock<HttpWebResponse>(MockBehavior.Loose);
            response.Setup(c => c.GetResponseStream()).Returns(responseStream);
            response.Setup(c => c.StatusCode).Returns(((HttpStatusCode)(apiResponse.HttpCode ?? config.DefaultHttpCode)));
            response.Setup(c => c.ContentLength).Returns(apiResponse.ContentLength ?? config.DefaultContentLength);
            response.Setup(c => c.ContentType).Returns(apiResponse.ContentType ?? config.DefaultContentType);
            response.Setup(c => c.Method).Returns(apiResponse.Method ?? config.DefaultMethod);

            Uri uri = null;
            if(!string.IsNullOrEmpty(apiResponse.ResponseUri))
            {
                uri = new Uri(apiResponse.ResponseUri);
            }
            else if(!string.IsNullOrEmpty(config.DefaultResponseUri))
            {
                uri = new Uri(config.DefaultResponseUri);
            }

            response.Setup(c => c.ResponseUri).Returns(uri);
            response.Setup(c => c.StatusDescription).Returns(apiResponse.StatusDescription ?? config.DefaultStatusDescription);
            response.Setup(c => c.IsFromCache).Returns(apiResponse.IsFromCache ?? config.DefaultIsFromCache);
            response.Setup(c => c.IsMutuallyAuthenticated).Returns(apiResponse.IsMutuallyAuthenticated ?? config.DefaultIsMutuallyAuthenticated);
            response.Setup(c => c.Cookies).Returns(apiResponse.CookieCollection ?? config.CookieCollection);
            response.Setup(c => c.Headers).Returns(apiResponse.HeaderCollection ?? config.DefaultHeaderCollection);

            return response;
        }

        private static MemoryStream CreateResponseStream(Config config, HttpResponseMock apiResponse)
        {
            byte[] expectedBytes;

            var responseStream = new MemoryStream();

            if (apiResponse.HasFile)
            {
                if(!File.Exists(apiResponse.File))
                {
                    throw new FileLoadException($"File {apiResponse.File} not exists");
                }
                expectedBytes = Encoding.UTF8.GetBytes(File.ReadAllText(apiResponse.File));
            }
            else
            {
                expectedBytes = Encoding.UTF8.GetBytes(apiResponse.Response ?? config.DefaultResponse);
            }

            responseStream.Write(expectedBytes, 0, expectedBytes.Length);
            responseStream.Seek(0, SeekOrigin.Begin);
            return responseStream;
        }
    }
}
