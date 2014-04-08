using AzureVideoStreaming.Phone.Models;
using AzureVideoStreaming.Phone.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace AzureVideoStreaming.Phone.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IAzureVideoService azureVideoService;
        private bool isLoading;
        private ObservableCollection<Video> videos;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IAzureVideoService azureVideoService)
        {
            this.azureVideoService = azureVideoService;
            this.GoToVideoCommand = new RelayCommand<object>(NavigateToVideoDetailPage);
            this.IsLoading = true;
            this.Videos = new ObservableCollection<Video>();

            if (IsInDesignMode)
            {
                var videos = azureVideoService.GetAllVideosAsync().Result;
                this.Videos.Clear();

                foreach(var video in videos)
                {
                    this.Videos.Add(video);
                }
            }
            else
            {
                // Code runs "for real"
            }
        }

        public ICommand GoToVideoCommand
        {
            get;
            private set;
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

        public ObservableCollection<Video> Videos
        {
            get
            {
                return videos;
            }
            set
            {
                this.videos = value;
                base.RaisePropertyChanged("Videos");
            }
        }

        public async Task NavigatedToAsync(NavigationEventArgs e)
        {
            List<Video> listOfVideos = null;
            try
            {
                listOfVideos = await this.azureVideoService.GetAllVideosAsync();
            }
            catch
            {
                this.IsLoading = false;
            }
            finally
            {
            }

            if (listOfVideos != null)
            {
                this.Videos.Clear();

                foreach (var video in listOfVideos)
                {
                    if (!String.IsNullOrEmpty(video.ThumbnailUrl) && !String.IsNullOrEmpty(video.UrlVc1))
                    {
                        this.Videos.Add(video);
                    }
                }
            }

            this.IsLoading = false;
        }

        private void NavigateToVideoDetailPage(object param = null)
        {
            var video = param as Video;
            var frame = (PhoneApplicationFrame)((App.Current as App).RootVisual);
            frame.Navigate(new Uri(String.Format("/VideoDetailPage.xaml?id={0}", video.RowKey), UriKind.Relative));
        }
    }
}