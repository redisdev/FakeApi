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

            serviceCollection.AddScoped<IHttpRequester, FakeHttpRequester>(provider =>
            {
                return new FakeHttpRequester("Config/Api/api.cfg.json");
            });

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var httpRequester = serviceProvider.GetService<IHttpRequester>();

            //Users apis
            var getUserByIdRequest = WebRequest.Create("https://localhost/api/users/128");
            var getUserByIdResponse = httpRequester.GetResponse(getUserByIdRequest);

            using (var stream = new StreamReader(getUserByIdResponse.GetResponseStream()))
            {
                var serializer = new JsonSerializer();
                var user = (User)serializer.Deserialize(stream, typeof(User));

                Console.WriteLine("Data from https://localhost/api/users/128 : ");
                Console.WriteLine($"UserId : {user.Id} | Firstname : {user.Firstname} | Lastname : {user.Lastname}");
            }

            Console.Read();
        }
    }
}
