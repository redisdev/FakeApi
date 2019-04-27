# What is FakeApi?

FakeApi provides the ability to send HttpWebRequest and get HttpWebResponses without a server.

# When use FakeApi?

- You are waiting for your backend colleagues to finish developing the Web Api?
- You need to quickly do a demo of a software and you do not have the time to develop Web Api?
FakeApi is for you!

# How to use FakeApi?

### FakeApi configuration file

First of all add to your project a json configuration file named as you like (don't forget to copy to output folder). 

- #### HttpWebResponse default values configuration

In this  file, you can define default values for all properties available in HttpWebResponse .Net and object than will be use by FakeApi to build responses when you are sending HttpWebRequest. These values will be used by default for all apis that you will configure in this file and must be declare at the root path like bellow:
```json
{
  "defaultDelay": 500,
  "defaultHttpCode": 200,
  "defaultMethod": "GET"
}
```
- #### Web Api configuration

Now, you can define your web api configuration:

```json
"apis": [
    {
      "url": "http://localhost:8080/api/users/{idUser}",
      "responses": [
        {
          "active": 1,
          "delay": 300,
          "httpCode": 200,
          "response": "{ 'firstname': 'john', 'lastname':'doe', 'id': 567 }"
        }
      ]
    }
```
For each web api you can set many responses. The first response flagged as active (active = 1) will be used by FakeApi.
Note that you can use template segment in your url configuration (/{idUser}).
You can also override all default values defined at the root path in each apis configuration.

- #### How to return data from file?

Simply set the "file" property into your api configuration:

```json
{
      "url": "http://localhost:8080/api/get-file",
      "responses": [
        {
          "active": 1,
          "httpCode": 200,
          "file": "DownloadFile.txt"
        }
      ]
    }
```
- #### How to throw a WebException?

Maybe you will have to test how your code reacts when a web exception are throwing. To do that, you need to set the "webExceptionMessage" property:

```json
{
      "url": "http://localhost:8080/api/ad/{adId}",
      "responses": [
        {
          "active": 1,
          "httpCode": 400,
          "webExceptionMessage": "Invalid ad id"
        }
      ]
}
````

- #### How to throw a custom exception?

You can force FakeApi to throw your exceptions:

```json
{
      "url": "http://localhost:8080/api/ad/{adId}/custom-error",
      "responses": [
        {
          "active": 1,
          "httpCode": 500,
          "customApiException": 
          {
            "fullTypeName": "App.CustomWebException, App",
            "constructorArgs": [
              "custom exception message"
            ]
          }
        }
      ]
}
```

- #### Create a HttpWebRequest and use FakeApi

Finally you have to create your web request and use the FakeHttpRequester provided by FakeApi to get the corresponding HttpWebResponse:

```csharp

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
```


