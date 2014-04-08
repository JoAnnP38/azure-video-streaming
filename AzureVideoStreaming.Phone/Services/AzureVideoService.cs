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
        
        public async Task<List<Comment>> GetCommentsAsync(string id)
        {
            var requestUrl = baseUrl + String.Format("Video/GetComments?videoid={0}", id);
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();
                var responseContentAsString = await response.Content.ReadAsStringAsync();
                return await JsonConvert.DeserializeObjectAsync<List<Comment>>(responseContentAsString);
            }
        }

        public async Task<Like> GetLikes(string id, string userId = null)
        {
            var requestUrl = baseUrl + String.Format("Video/GetLikes?videoid={0}&userid=null", id);
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();
                var responseContentAsString = await response.Content.ReadAsStringAsync();
                return await JsonConvert.DeserializeObjectAsync<Like>(responseContentAsString);
            }
        }

        public async Task<Video> GetVideoAsync(string id)
        {
            var requestUrl = baseUrl + String.Format("Video/Get?videoid={0}", id);
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();
                var responseContentAsString = await response.Content.ReadAsStringAsync();
                return await JsonConvert.DeserializeObjectAsync<Video>(responseContentAsString);
            }
        }
    }
}
