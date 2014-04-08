using AzureVideoStreaming.Phone.Models;
using AzureVideoStreaming.Phone.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Controls;
using System;
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
        private ObservableCollection<Video> videos;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IAzureVideoService azureVideoService)
        {
            this.azureVideoService = azureVideoService;
            this.GoToVideoCommand = new RelayCommand(NavigateToVideoDetailPage);
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
            var listOfVideos = await this.azureVideoService.GetAllVideosAsync();
            this.Videos.Clear();

            foreach(var video in listOfVideos)
            {
                this.Videos.Add(video);
            }
        }

        private void NavigateToVideoDetailPage()
        {
            var frame = (PhoneApplicationFrame)((App.Current as App).RootVisual);
            frame.Navigate(new Uri("/VideoDetailPage.xaml", UriKind.Relative));
        }
    }
}