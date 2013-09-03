using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace ViewNote
{
    public partial class AddNote : PhoneApplicationPage
    {
        public AddNote()
        {
            InitializeComponent();
        }

        private void appbarCheck_Click(object sender, EventArgs e)
        {
            if ( NavigationService.CanGoBack )
            {
                NavigationService.GoBack();
            };
        }

        private void appbarHelp_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/HelpPage.xaml", UriKind.Relative));
        }

        private void appbarCancel_Click(object sender, EventArgs e)
        {
            if ( NavigationService.CanGoBack )
            {
                NavigationService.GoBack();
            }
        }
    }
}