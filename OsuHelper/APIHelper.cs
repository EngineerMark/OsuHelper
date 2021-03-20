using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;

namespace OsuApiHelper
{
    public static class APIHelper<T>
    {
        private static List<Tuple<string, int, string>> cachedApiData = new List<Tuple<string, int, string>>();

        public static bool IsUrlValid(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";
            try
            {
                request.GetResponse();
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public static T GetData(string url, bool useCache = false)
        {
            string data = string.Empty;
            if (useCache && IsCached(url))
                data = ReadCache(url);

            if (string.IsNullOrEmpty(data))
            {
                data = GetDataFromWeb(url);
                if(useCache)
                    WriteCache(url, data);                
            }
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static string GetDataFromWeb(string url)
        {
            WebClient client = new WebClient();
            string s = client.DownloadString(url);
            return s;
        }

        private static void WriteCache(string url, string data)
        {
            string encodedUrl = url.Base64Encode();
            string path = GetDirectory();
            string fullPath = Path.Combine(path, encodedUrl, ".cache");
            if(File.Exists(fullPath))
                File.Delete(fullPath);
            (new FileInfo(fullPath)).Directory.Create();
            using (StreamWriter file = new StreamWriter(fullPath, false))
                file.Write(data);
        }

        private static string ReadCache(string url)
        {
            string encodedUrl = url.Base64Encode();
            string path = GetDirectory();
            string fullPath = Path.Combine(path, encodedUrl, ".cache");
            string data = string.Empty;
            if (File.Exists(fullPath))
                using (StreamReader file = new StreamReader(fullPath))
                    data = file.ReadToEnd();

            return data;
        }

        private static bool IsCached(string url)
        {
            string encodedUrl = url.Base64Encode();
            string path = GetDirectory();
            string fullPath = Path.Combine(path, encodedUrl, ".cache");
            return File.Exists(fullPath);
        }

        private static string GetDirectory()
        {
            string path = Path.Combine(Environment.CurrentDirectory,"cache");
            return path;
        }
    }
}