using System;
using System.IO;
using System.Net;
using FakeApi;
using Newtonsoft.Json;

namespace App
{
    public static class UserRequestExample
    {
        public static void Start(IHttpRequester httpRequester)
        {
            PostUser(httpRequester);
            GetUserById(httpRequester);
            //Multiple results files
            SearchUser(httpRequester);
            SearchUser(httpRequester);
            SearchUser(httpRequester);
            SearchUser(httpRequester);
        }

        private static void SearchUser(IHttpRequester httpRequester)
        {
            var searchRequest = WebRequest.Create("https://localhost/api/users?pIndex=0&pSize=2");
            searchRequest.Method = "GET";
            var searchResponse = httpRequester.GetResponse(searchRequest);

            using (var stream = new StreamReader(searchResponse.GetResponseStream()))
            {
                var serializer = new JsonSerializer();
                var searchUsers = (SearchUsers)serializer.Deserialize(stream, typeof(SearchUsers));
                Console.WriteLine("Data from GET https://localhost/api/users?pIndex=0&pSize=2");

                foreach (var user in searchUsers.Results)
                {
                    LogUser(user);
                }

                LogCookiesAndHeaders(searchResponse);
            }
        }

        private static void GetUserById(IHttpRequester httpRequester)
        {
            var getUserByIdRequest = WebRequest.Create("https://localhost/api/users/128");
            getUserByIdRequest.Method = "GET";
            var getUserByIdResponse = httpRequester.GetResponse(getUserByIdRequest);

            using (var stream = new StreamReader(getUserByIdResponse.GetResponseStream()))
            {
                var serializer = new JsonSerializer();
                var user = (User)serializer.Deserialize(stream, typeof(User));

                Console.WriteLine("Data from GET https://localhost/api/users/128");
                LogUser(user);
                LogCookiesAndHeaders(getUserByIdResponse);
            }
        }

        private static void PostUser(IHttpRequester httpRequester)
        {
            //Post user
            var postUserRequest = WebRequest.Create("https://localhost/api/users");
            postUserRequest.Method = "POST";
            var postUserResponse = httpRequester.GetResponse(postUserRequest);

            using (var stream = new StreamReader(postUserResponse.GetResponseStream()))
            {
                var serializer = new JsonSerializer();
                var user = (User)serializer.Deserialize(stream, typeof(User));

                Console.WriteLine("Data from POST https://localhost/api/users : ");
                LogUser(user);
                LogCookiesAndHeaders(postUserResponse);
            }
        }

        private static void LogUser(User user)
        {
            Console.WriteLine($"UserId : {user.Id} | Firstname : {user.Firstname} | Lastname : {user.Lastname}");
        }

        private static void LogCookiesAndHeaders(HttpWebResponse httpWebResponse)
        {
            foreach(var cookie in httpWebResponse.Cookies)
            {
                Console.WriteLine(cookie);
            }

            foreach (var header in httpWebResponse.Headers)
            {
                Console.WriteLine(header);
            }
        }
    }
}
