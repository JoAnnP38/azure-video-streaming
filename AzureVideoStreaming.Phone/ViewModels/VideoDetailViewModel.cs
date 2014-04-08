using AzureVideoStreaming.Phone.Services;
using GalaSoft.MvvmLight;

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
        private bool isLoading;

        /// <summary>
        /// Initializes a new instance of the VideoDetailViewModel class.
        /// </summary>
        public VideoDetailViewModel(IAzureVideoService azureVideoService)
        {
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
    }
}