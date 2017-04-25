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
using NRI.FileManager.Models;

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
            var action = args.FirstOrDefault();
            if (action == "upload")
            {
                var taskUpload = Upload();
                taskUpload.Wait();
                Console.WriteLine(taskUpload.Result);
            }
            else
            {
                while (true)
                {
                    Console.Write("Input action:");
                    action = Console.ReadLine();
                    if (action == "upload")
                    {
                        try
                        {
                            Stopwatch st = new Stopwatch();
                            st.Start();
                            Console.WriteLine("Begin => " + action);
                            var taskUpload = Upload();
                            taskUpload.Wait();
                            st.Stop();
                            Console.WriteLine("End => " + action + " Usage Total Seconds => " + TimeSpan.FromMilliseconds(st.ElapsedMilliseconds).TotalSeconds);
                            Console.WriteLine("Results => " + JsonHelper.FormatJson(taskUpload.Result));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error => " + e.Message);
                        }
                    }
                    else if (action == "uploadtemp")
                    {
                        try
                        {
                            Stopwatch st = new Stopwatch();
                            st.Start();
                            Console.WriteLine("Begin => " + action);
                            var taskUpload = UploadTempFile();
                            taskUpload.Wait();
                            st.Stop();
                            Console.WriteLine("End => " + action + " Usage Total Seconds => " + TimeSpan.FromMilliseconds(st.ElapsedMilliseconds).TotalSeconds);
                            Console.WriteLine("Results => " + JsonHelper.FormatJson(taskUpload.Result));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error => " + e.Message);
                        }
                    }
                    else if (action == "copytempfile")
                    {
                        try
                        {
                            Stopwatch st = new Stopwatch();
                            st.Start();
                            Console.WriteLine("Begin => " + action);
                            var taskUpload = CopyTempFile();
                            taskUpload.Wait();
                            st.Stop();
                            Console.WriteLine("End => " + action + " Usage Total Seconds => " + TimeSpan.FromMilliseconds(st.ElapsedMilliseconds).TotalSeconds);
                            Console.WriteLine("Results => " + JsonHelper.FormatJson(taskUpload.Result));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error => " + e.Message);
                        }
                    }
                }
            }
        }

        public static async Task<string> Upload()
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    var rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    var files = new List<string> { Path.Combine(rootPath, "DSC04157.JPG"), Path.Combine(rootPath, "DSC04158.JPG") };

                    foreach (var file in files)
                    {
                        var filestream = new FileStream(file, FileMode.Open);
                        var fileName = Path.GetFileName(file);
                        content.Add(new StreamContent(filestream), "file", fileName);
                    }

                    // client.DefaultRequestHeaders.Add("ENTERPRISE_GUID", "7E01C680-9C26-4268-A183-407BB2B2D7B7");
                    // client.DefaultRequestHeaders.Add("ENTERPRISE_ID", "Hello-ID");
                    // client.DefaultRequestHeaders.Add("MODULE", "TestUpload");
                    content.Add(new StringContent("7E01C680-9C26-4268-A183-407BB2B2D7B7"), "enterprise_guid");
                    content.Add(new StringContent("GMO-002"), "enterprise_id");
                    content.Add(new StringContent("OpenAccount"), "module");
                    try
                    {
                        using (var message = await client.PostAsync("http://localhost:18002/common/fileupload/upload", content))
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

        public static async Task<string> UploadTempFile()
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    var rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    var files = new List<string> { Path.Combine(rootPath, "DSC04157.JPG"), Path.Combine(rootPath, "DSC04158.JPG") };

                    foreach (var file in files)
                    {
                        var filestream = new FileStream(file, FileMode.Open);
                        var fileName = Path.GetFileName(file);
                        content.Add(new StreamContent(filestream), "file", fileName);
                    }

                    client.DefaultRequestHeaders.Add("enterprise_id", "GMO-002");
                    client.DefaultRequestHeaders.Add("api_key", "gmo-002");

                    try
                    {
                        using (var message = await client.PostAsync("http://localhost:18001/cm/tempfileupload/uploads", content))
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

        public static async Task<string> CopyTempFile()
        {
            string enterpriseId = "GMO-002";
            string module = "OpenAccount";
            Guid enterpriseGUID = Guid.Parse("7E01C680-9C26-4268-A183-407BB2B2D7B7");
            string refCode = "9841F587-79BF-432E-AA92-EDFE458F0887";
            using (var client = new HttpClient())
            {
                var model = new UploadTempItem
                {
                    EnterpriseGUID = enterpriseGUID,
                    EnterpriseID = enterpriseId,
                    RefCode = refCode,
                    Module = module
                };

                string json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    var message = await client.PostAsync("http://localhost:18002/common/fileupload/copytempfile", content);
                    var input = await message.Content.ReadAsStringAsync();
                    return input;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}