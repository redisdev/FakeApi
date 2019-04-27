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

