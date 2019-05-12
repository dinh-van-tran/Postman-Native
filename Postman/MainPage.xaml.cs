using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DataAccessLibrary;
using Postman.Views.RequestView;
using Postman.Services;
using Windows.System.Threading;
using Windows.UI.Core;

namespace Postman
{
    public sealed partial class MainPage : Page
    {
        private Request currentRequest;

        public MainPage()
        {
            this.InitializeComponent();
            DataAccess.InitializeDatabase();
        }

        private void loadData()
        {
            var requests = DataAccess.getAllRequest();
            if (requests.Count == 0)
            {
                this.newRequest();
                return;
            }

            foreach (var request in requests)
            {
                RequestItem requestItem = new RequestItem();
                requestItem.Request = request;
                requestItem.Panel = this;
                this.requestPanel.Children.Add(requestItem);
            }

            this.SetRequestValue(requests[requests.Count - 1]);
        }

        public void SetRequestValue(Request request)
        {
            this.currentRequest = request;
            this.DataContext = request;

            this.methodControl.Value = request;
            this.headerParamPanel.Value = request.Headers;
            this.queryParamPanel.Value = request.QueryParameters;
            this.bodyParamPanel.Value = request;
        }

        private void sendButtonClick(object sender, RoutedEventArgs e)
        {
            ThreadPool.RunAsync((workItem) =>
            {
                Response response = RestClient.SendRequestAsync(this.currentRequest).Result;
                Dispatcher.RunAsync(CoreDispatcherPriority.High, new DispatchedHandler(() =>
                {
                    sendRequestComplete(response);
                }));
            });
        }

        private void sendRequestComplete(Response response)
        {
            this.statusText.Text = Convert.ToString(response.StatusCode);
            this.responseBodyTextBox.Text = response.Content;
            this.elapsedTimeText.Text = Convert.ToString(response.ElapsedTime) + "ms";
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentRequest.Name == "")
            {
                var saveDialog = new SaveRequestDialog(this);
                var result = await saveDialog.ShowAsync();
                if (result != ContentDialogResult.Primary)
                {
                    return;
                }

                this.currentRequest.Name = saveDialog.Text.Trim();
            }

            this.SaveRequest();
        }

        public void SaveRequest()
        {
            int id = DataAccess.saveRequest(this.currentRequest);

            if (this.currentRequest.Id == -1)
            {
                this.currentRequest.Id = id;
                RequestItem requestItem = new RequestItem();
                requestItem.Request = this.currentRequest;
                requestItem.Panel = this;
                this.requestPanel.Children.Add(requestItem);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.loadData();
        }

        private void NewRequestButton_Click(object sender, RoutedEventArgs e)
        {
            this.newRequest();
        }

        private void newRequest()
        {
            this.currentRequest = new Request();
            this.DataContext = this.currentRequest;
            this.SetRequestValue(this.currentRequest);
        }
    }
}
