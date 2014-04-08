using AzureVideoStreaming.Phone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureVideoStreaming.Phone.Services
{
    public interface IAzureVideoService
    {
        Task<List<Video>> GetAllVideosAsync();
        Task<Like> GetLikes(string id, string userId = null);
        Task<Video> GetVideoAsync(string id);
        Task<List<Comment>> GetCommentsAsync(string id);
    }
}
