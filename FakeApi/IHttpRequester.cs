using System.Net;
using System.Threading.Tasks;

namespace FakeApi
{
    /// <summary>
    /// Provides method to sending a web request
    /// </summary>
    public interface IHttpRequester
    {
        /// <summary>
        /// Gets the web response for <paramref name="request"/>
        /// </summary>
        HttpWebResponse GetResponse(HttpWebRequest request);

        /// <summary>
        /// Gets the web response async for <paramref name="request"/>
        /// </summary>
        Task<HttpWebResponse> GetResponseAsync(HttpWebRequest request);
    }
}
