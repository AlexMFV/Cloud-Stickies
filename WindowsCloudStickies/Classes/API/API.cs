using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Windows;
using System.Net;

namespace WindowsCloudStickies
{
    public static class API
    {
        static HttpClient http = new HttpClient();

        public static void Initialize()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        //public static async Task<object> Fetch(RequestType type, ReadType rType, string route, Dictionary<string, string> parameters = null)
        //{
        //    try
        //    {
        //        http.BaseAddress = new Uri(Properties.Resources.LocalIP);
        //        http.DefaultRequestHeaders.Accept.Clear();
        //        http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Properties.Resources.JsonHeader));
        //
        //        HttpResponseMessage response = null;
        //        HttpContent content = null;
        //
        //        if (type == RequestType.POST || type == RequestType.PUT)
        //            content = new StringContent(new JavaScriptSerializer().Serialize(parameters));
        //
        //        //HttpContentHeaders teste1 = content.Headers;
        //        //string teste2 = await content.ReadFromJsonAsync<string>();
        //
        //        switch (type)
        //        {
        //            case RequestType.GET: response = await http.GetAsync(route); break;
        //            case RequestType.POST: response = await http.PostAsync(route, content); break;
        //            case RequestType.DELETE: response = await http.DeleteAsync(route); break;
        //            case RequestType.PUT: response = await http.PutAsync(route, content); break;
        //        }
        //
        //        object message = null;
        //
        //        if (response.IsSuccessStatusCode)
        //        {
        //            switch (rType)
        //            {
        //                case ReadType.String: message = await response.Content.ReadAsStringAsync(); break;
        //                case ReadType.Stream: message = await response.Content.ReadAsStreamAsync(); break;
        //                case ReadType.ByteArray: message = await response.Content.ReadAsByteArrayAsync(); break;
        //            }
        //        }
        //
        //        return message;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        public static async Task<object> Fetch(RequestType type, string route, Dictionary<string, string> parameters = null)
        {
            HttpMethod method;
            switch (type)
            {
                case RequestType.GET: method = HttpMethod.Get; break;
                case RequestType.POST: method = HttpMethod.Post; break;
                case RequestType.DELETE: method = HttpMethod.Delete; break;
                case RequestType.PUT: method = HttpMethod.Put; break;
                default: return false;
            }

            string ip;
#if DEBUG
            ip = Properties.Resources.LocalIP;
#else
            ip = Properties.Resources.ServerIP;
#endif

            try
            {

                ip = Properties.Resources.ServerIP;
                string test = ip + route;

                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage(method, ip + route))
                {
                    if (parameters == null)
                    {
                        using (var response = await client
                                .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                                .ConfigureAwait(false))
                        {
                            response.EnsureSuccessStatusCode();
                            return await response.Content.ReadAsStringAsync();
                        }
                    }
                    else
                    {
                        var json = JsonConvert.SerializeObject(parameters);
                        using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                        {
                            request.Content = stringContent;

                            using (var response = await client
                                .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                                .ConfigureAwait(false))
                            {
                                response.EnsureSuccessStatusCode();
                                return await response.Content.ReadAsStringAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }
    }
}
