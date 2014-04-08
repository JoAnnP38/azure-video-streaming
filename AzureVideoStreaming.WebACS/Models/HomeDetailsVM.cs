using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureVideoStreaming.WebACS.Models
{
    public class HomeDetailsVM
    {
        public Model.Video Video { get; set; }

        public List<Model.Comment> Comments { get; set; }

        public string VideoId { get; set; }

        public string Comment { get; set; }
    }
}