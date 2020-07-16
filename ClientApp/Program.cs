using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Clear();
            client.BaseAddress = new Uri("https://localhost:44349");

            // 1. without access_token will not access the service
            //    and return 401 .
            var resWithoutToken = client.GetAsync("/products-api").Result;

            Console.WriteLine($"Sending Request to /products-api , without token.");
            Console.WriteLine($"Result : {resWithoutToken.StatusCode}");

            //2. with access_token will access the service
            //   and return result.
            client.DefaultRequestHeaders.Clear();
            Console.WriteLine("\nBegin Auth....");
            var jwt = GetJwt();
            Console.WriteLine("End Auth....");
            Console.WriteLine($"\nToken={jwt}");

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");
            var resWithToken = client.GetAsync("/products-api").Result;

            Console.WriteLine($"\nSend Request to /products-api , with token.");
            Console.WriteLine($"Result : {resWithToken.StatusCode}");
            Console.WriteLine(resWithToken.Content.ReadAsStringAsync().Result);

            //3. visit no auth service 
            Console.WriteLine("\nNo Auth Service Here ");
            client.DefaultRequestHeaders.Clear();
            var res = client.GetAsync("/products-api/1").Result;

            Console.WriteLine($"Send Request to /products-api/1");
            Console.WriteLine($"Result : {res.StatusCode}");
            Console.WriteLine(res.Content.ReadAsStringAsync().Result);

            Console.Read();
        }

        private static string GetJwt()
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("https://localhost:44349");
            client.DefaultRequestHeaders.Clear();

            var res2 = client.GetAsync("/users-api?name=admin&pwd=123").Result;

            dynamic jwt = JsonConvert.DeserializeObject(res2.Content.ReadAsStringAsync().Result);

            return jwt.access_token;
        }
    }
}
