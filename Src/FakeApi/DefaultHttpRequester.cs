using System;
using System.Net;
using System.Threading.Tasks;

namespace FakeApi
{
    public class DefaultHttpRequester: IHttpRequester
    {
        public HttpWebResponse GetResponse(WebRequest request)
        {
            if(request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var response = request.GetResponse();
            return response as HttpWebResponse;
        }

        public async Task<HttpWebResponse> GetResponseAsync(WebRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var response = await request.GetResponseAsync();
            return response as HttpWebResponse;
        }
    }
}
