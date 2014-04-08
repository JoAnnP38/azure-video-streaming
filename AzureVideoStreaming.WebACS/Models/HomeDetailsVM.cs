using System.Collections.Generic;

namespace AzureVideoStreaming.WebACS.Models
{
    public class HomeDetailsVM
    {
        public Model.Video Video { get; set; }

        public List<Model.Comment> Comments { get; set; }

        public string VideoId { get; set; }

        public string Comment { get; set; }

        public int Likes { get; set; }

        public bool AlreadyLiked { get; set; }
    }
}