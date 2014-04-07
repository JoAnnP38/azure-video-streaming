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
        /// <summary>
        /// Initializes a new instance of the VideoDetailViewModel class.
        /// </summary>
        public VideoDetailViewModel(IAzureVideoService azureVideoService)
        {
        }
    }
}