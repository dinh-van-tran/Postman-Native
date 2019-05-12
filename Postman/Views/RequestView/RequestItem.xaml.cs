using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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

        private void setRequestValue()
        {
            if (this.Panel == null)
            {
                return;
            }

            this.Panel.SetRequestValue(this.request);
        }

        private void NameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.setRequestValue();
        }

        private void NameTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != Windows.System.VirtualKey.Delete)
            {
                return;
            }

            if (this.Panel == null)
            {
                return;
            }

            this.Panel.DeleteRequest(this.request);
        }
    }
}
