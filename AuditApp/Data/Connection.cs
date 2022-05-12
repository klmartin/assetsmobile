using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AuditApp.Data
{
    public class Connect : IDisposable
    {


        public async Task<string> GetAsync(string link, string token = null)
        {
            var uri = Defaults.ROOT + link;

            var httpClient = new HttpClient();

            var requestAccepts = httpClient.DefaultRequestHeaders.Accept;
            requestAccepts.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string content = string.Empty;

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            try
            {
                var response = await httpClient.GetAsync(uri);
                //response.EnsureSuccessStatusCode();
                if (response.StatusCode != System.Net.HttpStatusCode.BadRequest && response.StatusCode != System.Net.HttpStatusCode.InternalServerError)
                {
                    content = await response.Content.ReadAsStringAsync();
                    return content;
                }
                else
                {
                    content = await response.Content.ReadAsStringAsync();
                    throw new Exception(content);
                }

            }
            catch (Exception e)
            {
                throw e;
            }

            //if (content != string.Empty || content != null)
            //{
            //    return content;
            //}
            //else
            //{
            //    throw new HttpRequestException();
            //}
        }


        public async Task<string> PostAsync(string link, object data, string token = null)
        {
            var uri = Defaults.ROOT + link;

            var httpClient = new HttpClient();

            var requestAccepts = httpClient.DefaultRequestHeaders.Accept;
            requestAccepts.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var stringData = JsonConvert.SerializeObject(data);
            var response = await httpClient.PostAsync(uri, new StringContent(stringData, Encoding.UTF8, "application/json"));

            if (response.StatusCode != System.Net.HttpStatusCode.BadRequest && response.StatusCode != System.Net.HttpStatusCode.InternalServerError)
            {
                string content = await response.Content.ReadAsStringAsync();
                return content;
            }
            else
            {
                string content = await response.Content.ReadAsStringAsync();
                throw new Exception(content);
            }

        }

        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {

            if (disposing)
            {

            }


        }

        public async Task<string> Login(string link, List<KeyValuePair<string, string>> data)
        {
            var uri = Defaults.ROOT + link;

            var httpClient = new HttpClient();
            var requestAccepts = httpClient.DefaultRequestHeaders.Accept;
            requestAccepts.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            httpClient.BaseAddress = new Uri(Defaults.ROOT);

            var request = new HttpRequestMessage(HttpMethod.Post, link);
            request.Content = new FormUrlEncodedContent(data);
            // var stringData = JsonConvert.SerializeObject(data);
            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            return content;
        }

        public async Task<string> Upload(string link, byte[] file, string filename, string token = null)
        {
            var uri = Defaults.ROOT + link;

            var httpClient = new HttpClient();
            var requestAccepts = httpClient.DefaultRequestHeaders.Accept;
            requestAccepts.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            httpClient.BaseAddress = new Uri(Defaults.ROOT);

            var request = new HttpRequestMessage(HttpMethod.Post, link);
            //request.Content = new FormUrlEncodedContent(data);

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            MultipartFormDataContent form = new MultipartFormDataContent();
            //var stringData = JsonConvert.SerializeObject(data);
            //var content = new StringContent("picture");
            var content = new StreamContent(new MemoryStream(file));
            //content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            //{
            //    Name = "file",
            //    FileName = "document." + extension
            //};


            form.Add(content, "file", filename);

            var response = await httpClient.PostAsync(uri, form);
            string _contentResp = await response.Content.ReadAsStringAsync();
            return _contentResp;
        }
    }
}