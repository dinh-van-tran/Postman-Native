using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DataAccessLibrary;
using Postman.Views.RequestView;

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
            currentRequest = new Request();
            List<Request> requests = DataAccess.getAllRequest();
            if (requests.Count == 0)
            {
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
            this.queryParamPanel.Value = request.QueryParameters;
            this.headerParamPanel.Value = request.Headers;
        }

        private void sendButtonClick(object sender, RoutedEventArgs e)
        {
            List<Parameter> queryParam = this.queryParamPanel.Value;
            StringBuilder sb = new StringBuilder();
            sb.Append("?");
            foreach(var param in queryParam)
            {
                sb.Append(param.Name).Append("=").Append(param.Value).Append("&");
            }

            this.responseBodyTextBox.Text = sb.ToString();
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
            this.currentRequest = new Request();
            this.DataContext = this.currentRequest;
            this.SetRequestValue(this.currentRequest);
        }
    }
}
