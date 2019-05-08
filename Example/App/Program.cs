using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using FakeApi;
using Microsoft.Extensions.DependencyInjection;

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

            UserRequestExample.Start(httpRequester);

            Console.Read();
        }
    }
}
