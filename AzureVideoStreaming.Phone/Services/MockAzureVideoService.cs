using AzureVideoStreaming.Phone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureVideoStreaming.Phone.Services
{
    public class MockAzureVideoService : IAzureVideoService
    {
        public Task<List<Video>> GetAllVideosAsync()
        {
            List<Video> videos = new List<Video>();
            videos.Add(new Video
                {
                    ThumbnailUrl = "/Assets/MockImages/mockthumb.jpg"
                }
            );

            videos.Add(new Video
                {
                    ThumbnailUrl = "/Assets/MockImages/mockthumb.jpg"
                }
            );

            videos.Add(new Video
                {
                    ThumbnailUrl = "/Assets/MockImages/mockthumb.jpg"
                }
            );

            videos.Add(new Video
                {
                    ThumbnailUrl = "/Assets/MockImages/mockthumb.jpg"
                }
            );

            videos.Add(new Video
                {
                    ThumbnailUrl = "/Assets/MockImages/mockthumb.jpg"
                }
            );

            videos.Add(new Video
                {
                    ThumbnailUrl = "/Assets/MockImages/mockthumb.jpg"
                }
            );

            return Task.FromResult<List<Video>>(videos);
        }

        public Task<Video> GetVideoAsync(string id)
        {
            return Task.FromResult<Video>(null);
        }

        public Task<List<Comment>> GetCommentsAsync(string id)
        {
            return Task.FromResult<List<Comment>>(null);
        }

        public Task<Like> GetLikes(string id, string userId = null)
        {
            return Task.FromResult<Like>(null);
        }
    }
}
