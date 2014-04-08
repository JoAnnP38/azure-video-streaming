using AzureVideoStreaming.Phone.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureVideoStreaming.Phone.Services
{
    public class AzureVideoService : IAzureVideoService
    {
        private readonly string baseUrl = "http://azurevideostreaming.cloudapp.net/";
        //private readonly string baseUrl = "http://localhost:10682/";
        public async Task<List<Video>> GetAllVideosAsync()
        {
            var requestUrl = baseUrl + "Video/GetAll";
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();
                var responseContentAsString = await response.Content.ReadAsStringAsync();
                return await JsonConvert.DeserializeObjectAsync<List<Video>>(responseContentAsString);
            }
        }
    }
}
