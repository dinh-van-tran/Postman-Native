using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using DataAccessLibrary;
// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Postman
{
    public sealed partial class RequestItem : UserControl
    {
        private WeakReference<MainPage> mainPanel;
        public MainPage Panel
        {
            get
            {
                MainPage result;
                if (mainPanel == null || !mainPanel.TryGetTarget(out result))
                {
                    return null;
                }

                return result;
            }
            set
            {
                if (mainPanel != null)
                {
                    mainPanel.SetTarget(value);
                }
                else
                {
                    mainPanel = new WeakReference<MainPage>(value);
                }
            }
        }

        private Request request;

        public Request Request
        {
            get { return request; }
            set {
                request = value;
                this.DataContext = value;
            }
        }

        public RequestItem()
        {
            this.InitializeComponent();
            this.request = new Request();
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.setRequestValue();
        }

        private void IdTextBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.setRequestValue();
        }

        private void NameTextBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.setRequestValue();
        }

        private void setRequestValue()
        {
            if (this.Panel == null)
            {
                return;
            }

            this.Panel.SetRequestValue(this.request);
        }

        private void IdTextBox_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.setRequestValue();
        }

        private void NameTextBox_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.setRequestValue();
        }

        private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.setRequestValue();
        }

        private void NameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.setRequestValue();
        }
    }
}
