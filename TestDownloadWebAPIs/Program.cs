using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Diagnostics;

using System.Net;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;

namespace TestPostMessageMulitpart
{
    class Program
    {
        private static string _downloadAction = "download";

        static void Main(string[] args)
        {
            var action = args.FirstOrDefault();
            if (action == _downloadAction)
            {
                DownloadFile(action);
            }
            else
            {
                while (true)
                {
                    Console.Write("Input action:");
                    action = Console.ReadLine();
                    DownloadFile(action);
                }
            }
        }

        private static void DownloadFile(string action)
        {
            if (action == _downloadAction)
            {
                try
                {
                    Stopwatch st = new Stopwatch();
                    st.Start();
                    Console.WriteLine("Begin => " + action);
                    var task = DownloadFileStream();
                    task.Wait();
                    st.Stop();
                    Console.WriteLine("End => " + action + " Usage Total Seconds => " + TimeSpan.FromMilliseconds(st.ElapsedMilliseconds).TotalSeconds);
                    Console.WriteLine("Results => " + task.Result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error => " + e.Message);
                }
            }
        }

        public static async Task<HttpResponseMessage> DownloadFileStream()
        {
            //string id = "E80A64E1-CCCD-42C4-9DD3-AEDEB441F2A1";
            string id = "B5BC2687-BC79-4A38-803C-3FDDAD2C14A6";
            var path = @"C:\BrokerFiles\TestDownload";
            var respond = new HttpResponseMessage();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var url = $"http://localhost:18002/v1/downloadstream?id=" + id;
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    using (HttpContent content = response.Content)
                    {
                        var filename = content.Headers.GetValues("content-disposition").ToArray();
                        var fileExtension = filename.First().Split(';')[1].Split('=')[1];

                        var filePath = Path.Combine(path, $"{Guid.NewGuid().ToString("N")}{Path.GetExtension(fileExtension)}");
                        Console.WriteLine("Downloaded File => " + filePath);
                        var byteResult = await content.ReadAsByteArrayAsync();
                        using (var ms = new MemoryStream(byteResult))
                        {
                            ms.Position = 0;
                            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                            {
                                ms.CopyTo(fs);
                                fs.Close();
                            }
                            ms.Close();
                        }
                    }
                    respond.StatusCode = response.StatusCode;
                }
            }

            return respond;
        }
    }
}