using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace less_1
{
    internal class Program
    {
        static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            StreamWriter sw = new StreamWriter("result.txt");

            var tasks = new List<Task<string>>();
            try
            {
                for (int i = 4; i <= 13; i++)
                {
                    var task_i = GetPost(i);
                    tasks.Add(task_i);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            await Task.WhenAll(tasks);

            tasks.ForEach(responseBody => { sw.WriteLine(responseBody.Result); });

            sw.Close();

            //Console.ReadKey();
        }

        static async Task<string> GetPost(int id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://jsonplaceholder.typicode.com/posts/" + id);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseBody);
                //Console.WriteLine(responseBody + "\n" + Thread.CurrentThread.ManagedThreadId);
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return string.Empty;
        }
    }
}
