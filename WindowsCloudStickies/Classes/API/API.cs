using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WindowsCloudStickies
{
    public static class API
    {
        static HttpClient http = new HttpClient();

        public static async Task<string> Fetch(RequestType type, string route, List<string> parameters = null)
        {
            http.BaseAddress = new Uri(Properties.Resources.LocalIP);
            http.DefaultRequestHeaders.Accept.Clear();
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Properties.Resources.JsonHeader));

            HttpResponseMessage response = http.GetAsync(route).Result;

            string message = "";
            
            if (response.IsSuccessStatusCode)
            {
                message = await response.Content.ReadAsStringAsync();
            }
            
            return message;
        }
    }
}
