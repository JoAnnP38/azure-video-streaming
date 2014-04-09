using AzureVideoStreaming.Phone.Models;
using AzureVideoStreaming.Phone.Services;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace AzureVideoStreaming.Phone.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class VideoDetailViewModel : ViewModelBase
    {
        private IAzureVideoService azureVideoService;
        private List<Comment> comments;
        private bool isLoading;
        private bool hasLikes = false;
        private Like like;
        private Video video;

        /// <summary>
        /// Initializes a new instance of the VideoDetailViewModel class.
        /// </summary>
        public VideoDetailViewModel(IAzureVideoService azureVideoService)
        {
            this.azureVideoService = azureVideoService;
            this.IsLoading = true;
        }

        public List<Comment> Comments
        {
            get
            {
                return comments;
            }
            set
            {
                comments = value;
                RaisePropertyChanged("Comments");
            }
        }

        public bool HasLikes
        {
            get
            {
                return hasLikes;
            }
            set
            {
                this.hasLikes = value;
                base.RaisePropertyChanged("HasLikes");
            }
        }

        public bool IsLoading
        {
            get
            {
                return isLoading;
            }
            set
            {
                this.isLoading = value;
                base.RaisePropertyChanged("IsLoading");
            }
        }

        public Like Like
        {
            get
            {
                return like;
            }
            set
            {
                this.like = value;
                base.RaisePropertyChanged("Like");
            }
        }

        public Video Video
        {
            get
            {
                return video;
            }
            set
            {
                this.video = value;
                base.RaisePropertyChanged("Video");
            }
        }

        public async Task NavigatedToAsync(NavigationEventArgs e, IDictionary<string,string> param = null)
        {
            this.HasLikes = false;
            this.Comments = null;
            this.Video = null;
            this.Like = null;

            Video video = null;
            string id = null;
            if (param != null)
            {
                if (param.ContainsKey("id"))
                {
                    id = param["id"];
                }
            }

            if (String.IsNullOrEmpty(id))
            {
                return;
            }
            
            try
            {
                this.Video = await this.azureVideoService.GetVideoAsync(id);
            }
            catch
            {
                this.IsLoading = false;
            }
            finally
            {
            }

            try
            {
                this.Comments = await this.azureVideoService.GetCommentsAsync(id);
            }
            catch
            {
                this.IsLoading = false;
            }
            finally
            {
            }

            try
            {
                this.Like = await this.azureVideoService.GetLikes(id, null);
                if (this.Like != null && this.Like.Count > 0)
                {
                    this.HasLikes = true;
                }
            }
            catch
            {

            }
            finally
            {
                this.IsLoading = false;
            }
        }
    }
}