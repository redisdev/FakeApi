using System;
using System.IO;
using System.Net;
using FakeApi;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();

#if DEBUG
            serviceCollection.AddScoped<IHttpRequester, FakeHttpRequester>(provider =>
            {
                return new FakeHttpRequester("api.cfg.json");
            });
#endif

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var httpRequester = serviceProvider.GetService<IHttpRequester>();

            //Get user request
            var getUserRequest = (HttpWebRequest)WebRequest.Create("http://localhost:8080/api/users/56");
            var getUserResponse = httpRequester.GetResponse(getUserRequest);

            using (var stream = new StreamReader(getUserResponse.GetResponseStream()))
            {
                var data = stream.ReadToEnd();
                var user = JsonConvert.DeserializeObject<User>(data);
                Console.WriteLine($"json data from {getUserRequest.RequestUri}");
                Console.WriteLine($"Firstname : {user.Firstname} | Lastname : {user.Lastname} | Id : {user.Id}");
            }

            Console.WriteLine();

            //Get order address
            var getOrderAddressRequest = (HttpWebRequest)WebRequest.Create("http://localhost:8080/api/orders/666/addresses/2");
            var getOrderAddressResponse = httpRequester.GetResponse(getOrderAddressRequest);

            using (var stream = new StreamReader(getOrderAddressResponse.GetResponseStream()))
            {
                var data = stream.ReadToEnd();
                var orderAddress = JsonConvert.DeserializeObject<OrderAddress>(data);
                Console.WriteLine($"json data from {getOrderAddressRequest.RequestUri}");
                Console.WriteLine($"OrderId : {orderAddress.OrderId} | Street Address : {orderAddress.StreetAddress} | Id : {orderAddress.City}");

                foreach (var cookie in getOrderAddressResponse.Cookies)
                {
                    Console.WriteLine($"Cookie: {cookie}");
                }

                foreach (var header in getOrderAddressResponse.Headers)
                {
                    Console.WriteLine($"Header: {header}");
                }
            }

            Console.WriteLine();

            //Get error
            var getErrorRequest = (HttpWebRequest)WebRequest.Create("http://localhost:8080/api/ad/56");

            try
            {
                Console.WriteLine($"Send request to {getErrorRequest.RequestUri}");
                var getErrorResponse = httpRequester.GetResponse(getErrorRequest);
            }
            catch (WebException ex)
            {
                Console.WriteLine($"HttpErrorCode : {ex.Status} | Message : {ex.Message}");
            }

            Console.WriteLine();

            //Get custom error
            var getCustomErrorRequest = (HttpWebRequest)WebRequest.Create("http://localhost:8080/api/ad/900/custom-error");

            try
            {
                Console.WriteLine($"Send request to {getCustomErrorRequest.RequestUri}");
                var getCustomErrorResponse = httpRequester.GetResponse(getCustomErrorRequest);
            }
            catch (CustomWebException ex)
            {
                Console.WriteLine($"Message : {ex.Message}");
            }

            Console.WriteLine();

            //Get file
            var getFileRequest = (HttpWebRequest)WebRequest.Create("http://localhost:8080/api/get-file");
            var getFileResponse = httpRequester.GetResponse(getFileRequest);

            using (var stream = new StreamReader(getFileResponse.GetResponseStream()))
            {
                Console.WriteLine($"Get file from {getFileRequest.RequestUri}");
                var data = stream.ReadToEnd();
                Console.WriteLine($"Data from file : {data}");
            }

            Console.WriteLine();
            Console.Read();
        }

    }
}
