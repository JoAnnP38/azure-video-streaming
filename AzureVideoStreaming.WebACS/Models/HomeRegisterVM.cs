using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AzureVideoStreaming.WebACS.Models
{
    public class HomeRegisterVM
    {
        [Required]
        public string Username { get; set; }
    }
}