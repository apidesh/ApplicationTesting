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
            string id = "1e5e55be-8cae-41c8-be62-24330cae1b66";

            var path = @"C:\\Temp\\";
            var respond = new HttpResponseMessage();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var url = $"http://localhost:18002/common/filedownload/downloadstream?id=" + id;
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    using (HttpContent content = response.Content)
                    {
                        if (content.Headers.Contains("content-disposition"))
                        {
                            var filename = content.Headers.GetValues("content-disposition")?.ToArray();
                            var fileExtension = WebUtility.UrlDecode(filename.First().Split(';')[1].Split('=')[1]);

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
                    }
                    respond.StatusCode = response.StatusCode;
                }
            }

            return respond;
        }
    }
}