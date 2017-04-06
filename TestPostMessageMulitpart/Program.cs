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

namespace TestPostMessageMulitpart
{
    public class FileItemInfo
    {
        public string EnterpriseGUID { get; set; }
        public string EnterpriseID { get; set; }
        public string RefCode { get; set; }
        public string Module { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var taskUpload = Upload();
            taskUpload.Wait();
            var resultUpload = taskUpload.Result;
            Console.WriteLine(resultUpload);

            // var taskUploadRefFile = UploadRefFile();
            // taskUploadRefFile.Wait();
            // var resultRefDoc = taskUploadRefFile.Result;
            //  Console.WriteLine(resultRefDoc);

            // var Uploadx = Upload(); var task = Upload(); task.Wait();

            //var result = task.Result;
            // Console.WriteLine(result);

            //var taskUploadRefCodeFile = UploadRefCodeFile();
            //taskUploadRefCodeFile.Wait();
            //var resultRefCodeFile = taskUploadRefCodeFile.Result;
            //Console.WriteLine(resultRefCodeFile);

            Task task = DownloadFileAsync();
            task.Wait();

            /*  var taskDownload = DownloadFile();
              taskDownload.Wait();
              //var byteArray = taskDownload.Result;

              if(taskDownload.Result != null)
              {
                  using (var ms = new MemoryStream())
                  {
                      ms.Position = 0;
                      taskDownload.Result.Content.CopyToAsync(ms);
                    //  taskDownload.Result.CopyTo(ms);
                      using (var fs = new FileStream(@"D:\Projects\temp-download\" + Guid.NewGuid().ToString("N"), FileMode.Create, FileAccess.Write))
                      {
                          ms.CopyTo(fs);
                          fs.Close();
                      }
                      ms.Close();
                  }
              }
              Console.ReadLine();*/
        }

        public static async Task<HttpResponseMessage> DownloadFileAsync()
        {
            string id = "6CA0BC1D-A298-4526-857F-C2CBE800A533";

            var url = $"http://localhost/filemanager/downloadfilesstream?id=" + id;
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    using (HttpContent content = response.Content)
                    {
                        var filename = content.Headers.GetValues("content-disposition").ToArray();
                        var fileExtension = filename.First().Split(';')[1].Split('=')[1];

                        var filePath = Path.Combine(@"D:\Projects\temp-download\", $"{Guid.NewGuid().ToString("N")}{Path.GetExtension(fileExtension)}");
                        // ... Read the string.
                        // string result = await content.ReadAsStringAsync();
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
            }

            return null;
        }

        public static async Task<HttpResponseMessage> DownloadPageAsync1()
        {
            // ... Target page.

            string id = "6CA0BC1D-A298-4526-857F-C2CBE800A533";

            var url = $"http://localhost/filemanager/downloadfilesstream?id=" + id;
            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    using (HttpContent content = response.Content)
                    {
                        // ... Read the string.
                        // string result = await content.ReadAsStringAsync();
                        var byteResult = await content.ReadAsByteArrayAsync();
                        using (var ms = new MemoryStream(byteResult))
                        {
                            ms.Position = 0;
                            using (var fs = new FileStream(@"D:\Projects\temp-download\" + Guid.NewGuid().ToString("N"), FileMode.Create, FileAccess.Write))
                            {
                                ms.CopyTo(fs);
                                fs.Close();
                            }
                            ms.Close();
                        }
                        // ... Display the result.
                        // if (result != null &&
                        //     result.Length >= 50)
                        // {
                        //     Console.WriteLine(result.Substring(0, 50) + "...");
                        // }
                    }
                }
            }

            return null;
        }

        private static void SaveFileStream(Stream stream)
        {
            using (var ms = new MemoryStream(GetBytesFromStream(stream)))
            {
                ms.Position = 0;
                using (var fs = new FileStream(@"D:\Projects\temp-download\" + Guid.NewGuid().ToString("N"), FileMode.Create, FileAccess.Write))
                {
                    ms.CopyTo(fs);
                    fs.Close();
                }
                ms.Close();
            }
        }

        private static byte[] GetBytesFromStream(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (stream.Length == 0)
                throw new ArgumentNullException("stream");

            stream.Seek(0, SeekOrigin.Begin);
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Close();
            return buffer;
        }

        public static async Task<string> Upload()
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    var rootPath = @"D:\Projects\TestApps\TestPostMessageMulitpart\";
                    var files = new List<string> { Path.Combine(rootPath, "52789252.jpg"), Path.Combine(rootPath, "52789251.jpg") };

                    foreach (var file in files)
                    {
                        var filestream = new FileStream(file, FileMode.Open);
                        var fileName = Path.GetFileName(file);
                        content.Add(new StreamContent(filestream), "file", fileName);
                    }

                    JsonContent jsonPart = new JsonContent(new FileItemInfo()
                    {
                        Module = "AccountOpen"
                    });
                    jsonPart.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                    jsonPart.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    try
                    {
                        using (var message = await client.PostAsync("http://localhost:18002/v1/upload", content))
                        {
                            var input = await message.Content.ReadAsStringAsync();
                            return input;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public static async Task<string> UploadRefFile()
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    var rootPath = @"D:\Projects\TestApps\TestPostMessageMulitpart\";
                    var files = new List<string> { Path.Combine(rootPath, "52789252.jpg"), Path.Combine(rootPath, "52789251.jpg") };

                    foreach (var file in files)
                    {
                        var filestream = new FileStream(file, FileMode.Open);
                        var fileName = Path.GetFileName(file);
                        content.Add(new StreamContent(filestream), "file", fileName);
                    }

                    try
                    {
                        using (var message = await client.PostAsync("http://localhost/filemanager/uploadreffile", content))
                        {
                            var input = await message.Content.ReadAsStringAsync();

                            return input;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        //public async static Task<BitmapImage> LoadImage(Uri uri)
        //{
        //    BitmapImage bitmapImage = new BitmapImage();

        //    try
        //    {
        //        using (HttpClient client = new HttpClient())
        //        {
        //            using (var response = await client.GetAsync(uri))
        //            {
        //                response.EnsureSuccessStatusCode();

        //                using (IInputStream inputStream = await response.Content.ReadAsInputStreamAsync())
        //                {
        //                    bitmapImage.SetSource(inputStream.AsStreamForRead());
        //                }
        //            }
        //        }
        //        return bitmapImage;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("Failed to load the image: {0}", ex.Message);
        //    }

        //    return null;
        //}
        public static Task<HttpResponseMessage> DownloadFile()
        {
            using (var client = new HttpClient())
            {
                // var content = new StringContent(items, Encoding.UTF8, "application/json");
                string id = "6CA0BC1D-A298-4526-857F-C2CBE800A533";
                try
                {
                    var url = $"http://localhost/filemanager/downloadfiles?id=" + id;

                    return client.GetAsync(url);
                    // MemoryStream ms = new MemoryStream();
                    // theStream.Position = 0;
                    //theStream.CopyTo(ms);
                    // StreamReader theStreamReader = new StreamReader(null);
                    // string theLine = null;

                    // while ((theLine = theStreamReader.ReadLine()) != null)
                    // {
                    // do something with the line
                    // }
                    //return new byte[] { };

                    //using (var message = await client.GetAsync(url))
                    //{
                    //    MemoryStream ms = new MemoryStream();

                    //    var input = await message.Content.ReadAsStreamAsync();
                    //    await input.CopyToAsync(ms);
                    //    //return input.CanRead;

                    //    return input;
                    //}
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task<string> UploadRefCodeFile()
        {
            using (var client = new HttpClient())
            {
                var itemInfo = new FileItemInfo()
                {
                    Module = "AccountOpen",
                    RefCode = "E00F077F-A7A9-4981-BDA7-26F1A0567735"
                };

                var itemList = new List<FileItemInfo>();
                itemList.Add(itemInfo);

                var items = JsonConvert.SerializeObject(itemList);

                var content = new StringContent(items, Encoding.UTF8, "application/json");

                try
                {
                    using (var message = await client.PostAsync("http://localhost/filemanager/uploadrefcodefile", content))
                    {
                        var input = await message.Content.ReadAsStringAsync();

                        return input;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private static void Upload2()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "CBS Brightcove API Service");

                // using (var content = new MultipartFormDataContent()) {
                var path = @"C:\B2BAssetRoot\files\596086\596086.1.mp4";

                string assetName = Path.GetFileName(path);

                //var request = new HTTPBrightCoveRequest()
                //{
                //    Method = "create_video",
                //    Parameters = new Params()
                //    {
                //        CreateMultipleRenditions = "true",
                //        EncodeTo = EncodeTo.Mp4.ToString().ToUpper(),
                //        Token = "x8sLalfXacgn-4CzhTBm7uaCxVAPjvKqTf1oXpwLVYYoCkejZUsYtg..",
                //        Video = new Video()
                //        {
                //            Name = assetName,
                //            ReferenceId = Guid.NewGuid().ToString(),
                //            ShortDescription = assetName
                //        }
                //    }
                //};

                var rootPath = @"D:\Projects\TestApps\TestPostMessageMulitpart\";
                // var message = new HttpRequestMessage();
                //var content = new MultipartFormDataContent();
                var files = new List<string> { Path.Combine(rootPath, "52789252.jpg"), Path.Combine(rootPath, "52789251.jpg") };

                //  foreach (var file in files)
                // {
                //     var filestream = new FileStream(file, FileMode.Open);
                //    var fileName = Path.GetFileName(file);
                //content.Add(new StreamContent(filestream), "file", fileName);
                // }

                //Content-Disposition: form-data; name="json"
                //var stringContent = new StringContent(JsonConvert.SerializeObject(request));
                //stringContent.Headers.Add("Content-Disposition", "form-data; name=\"json\"");
                //content.Add(stringContent, "json");

                //FileStream fs = File.OpenRead(path);

                //var streamContent = new StreamContent(fs);
                //streamContent.Headers.Add("Content-Type", "application/octet-stream");
                //streamContent.Headers.Add("Content-Disposition", "form-data; name=\"file\"; filename=\"" + Path.GetFileName(path) + "\"");
                //content.Add(streamContent, "file", Path.GetFileName(path));

                //content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");

                // Task message =
                // client.PostAsync("http://localhost:51896/common/api/filemanager/uploadfiles",
                // content); message.Wait();

                // var input = message.Wait(300); //message.Result.Content.ReadAsStringAsync();
                // Console.WriteLine(input); Console.Read(); }
            }
        }

        private static HttpContent CreateMultipartContent(JToken json, Stream file, string fileName)
        {
            MultipartContent content = new MultipartContent("form-data", Guid.NewGuid().ToString());

            JsonContent jsonPart = new JsonContent(json);
            jsonPart.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            jsonPart.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            StreamContent filePart = new StreamContent(file);
            filePart.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            filePart.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            filePart.Headers.ContentDisposition.FileName = fileName;

            content.Add(jsonPart);
            content.Add(filePart);

            return content;
        }

        private static HttpContent CreateMultipartContent(JToken json, string markupText, string fileName)
        {
            MultipartContent content = new MultipartContent("form-data", Guid.NewGuid().ToString());

            StringContent jsonPart = new StringContent(json.ToString());
            jsonPart.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            jsonPart.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            StringContent filePart = new StringContent(markupText);
            filePart.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            filePart.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            filePart.Headers.ContentDisposition.FileName = fileName;

            content.Add(jsonPart);
            content.Add(filePart);

            return content;
        }
    }
}