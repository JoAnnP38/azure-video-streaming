using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AzureVideoStreaming.Phone.ViewModels;

namespace AzureVideoStreaming.Phone
{
    public partial class VideoDetailPage : PhoneApplicationPage
    {
        public VideoDetailPage()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var viewModel = this.DataContext as VideoDetailViewModel;
            viewModel.NavigatedToAsync(e, NavigationContext.QueryString);
        }
    }
}