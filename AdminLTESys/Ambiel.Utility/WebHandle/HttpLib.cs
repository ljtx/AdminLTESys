using System;
using System.IO;
using System.Net;
using System.Text;

namespace Ambiel.Utility.WebHandle
{
    public class HttpLib
    {
        public static string HttpGetData(string Url, ref CookieContainer cookie, string referer = null)
        {
            int num = 0;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
            bool flag = cookie.Count == 0;
            if (flag)
            {
                httpWebRequest.CookieContainer = new CookieContainer();
                cookie = httpWebRequest.CookieContainer;
            }
            else
            {
                httpWebRequest.CookieContainer = cookie;
            }
            httpWebRequest.Method = "GET";
            httpWebRequest.Referer = referer;
            string result = string.Empty;
            httpWebRequest.ContentType = "text/html;charset=UTF-8";
            while (true)
            {
                try
                {
                    HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    Stream responseStream = httpWebResponse.GetResponseStream();
                    responseStream.ReadTimeout = 20000;
                    responseStream.WriteTimeout = 20000;
                    StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    result = streamReader.ReadToEnd();
                    streamReader.Close();
                    responseStream.Close();
                }
                catch (Exception ex)
                {
                    bool flag2 = ex.Message == "操作已超时";
                    if (flag2)
                    {
                        bool flag3 = num <= 15;
                        if (flag3)
                        {
                            num++;
                            continue;
                        }
                    }
                }
                break;
            }
            return result;
        }

        public static string HttpPostData(string Url, string postData, ref CookieContainer cookie, string ContentType = "text/html; charset=UTF-8", string refererUrl = null)
        {
            int num = 0;
            string result;
            while (true)
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
                bool flag = cookie.Count == 0;
                if (flag)
                {
                    httpWebRequest.CookieContainer = new CookieContainer();
                    cookie = httpWebRequest.CookieContainer;
                }
                else
                {
                    httpWebRequest.CookieContainer = cookie;
                }
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = ContentType;
                httpWebRequest.Referer = refererUrl;
                httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.104 Safari/537.36 Core/1.53.3538.400 QQBrowser/9.6.12501.400";
                result = "";
                try
                {
                    httpWebRequest.ContentLength = (long)postData.Length;
                    Stream requestStream = httpWebRequest.GetRequestStream();
                    StreamWriter streamWriter = new StreamWriter(requestStream, Encoding.GetEncoding("gb2312"));
                    streamWriter.Write(postData);
                    streamWriter.Close();
                    httpWebRequest.Timeout = 20000;
                    HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    Stream responseStream = httpWebResponse.GetResponseStream();
                    StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    result = streamReader.ReadToEnd();
                    streamReader.Close();
                    responseStream.Close();
                }
                catch (Exception)
                {
                    bool flag2 = num < 5;
                    if (flag2)
                    {
                        num++;
                        continue;
                    }
                }
                break;
            }
            return result;
        }

        public string webRequestGet(string url, out string cookie)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
            httpWebRequest.Method = "GET";
            string result;
            using (WebResponse response = httpWebRequest.GetResponse())
            {
                Stream responseStream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                StringBuilder stringBuilder = new StringBuilder();
                string value;
                while ((value = streamReader.ReadLine()) != null)
                {
                    stringBuilder.Append(value);
                }
                cookie = httpWebRequest.Headers.Get("Set-Cookie");
                streamReader.Close();
                responseStream.Close();
                result = stringBuilder.ToString();
            }
            return result;
        }

        public static bool HttpDownload(string url, string path)
        {
            string text = Environment.CurrentDirectory + "\\temp";
            Directory.CreateDirectory(text);
            string text2 = text + "\\" + Path.GetFileName(path) + ".temp";
            bool flag = File.Exists(text2);
            if (flag)
            {
                File.Delete(text2);
            }
            bool result=false;
            try
            {
                FileStream fileStream = new FileStream(text2, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
                HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                Stream responseStream = httpWebResponse.GetResponseStream();
                byte[] array = new byte[1024];
                for (int i = responseStream.Read(array, 0, array.Length); i > 0; i = responseStream.Read(array, 0, array.Length))
                {
                    fileStream.Write(array, 0, i);
                }
                fileStream.Close();
                responseStream.Close();
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.Move(text2, path);
                result = true;
            }
            catch (Exception e)
            {
              
            }
            return result;
        }
    }
}