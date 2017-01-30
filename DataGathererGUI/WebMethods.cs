using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataGathererGUI
{
    public static class WebMethods
    {
        public async static Task<string> WebGet(string url)
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                string data = await response.Content.ReadAsStringAsync();

                return data;
            }
            catch
            {
                return null;
            }
        }

        //public static void WebPost(string url, string jsonContent)
        //{
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //    request.Method = "POST";

        //    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        //    Byte[] byteArray = encoding.GetBytes(jsonContent);

        //    request.ContentLength = byteArray.Length;
        //    request.ContentType = @"application/json";

        //    using (Stream dataStream = request.GetRequestStream())
        //    {
        //        dataStream.Write(byteArray, 0, byteArray.Length);
        //    }
        //    long length = 0;
        //    try
        //    {
        //        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //        {
        //            length = response.ContentLength;
        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //        // Log exception and throw as for GET example above
        //    }
        //}
    }
}
