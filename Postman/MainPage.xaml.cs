using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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
            var url = this.urlTextbox.Text;

            this.currentRequest.Method = this.methodControl.Value;
            this.currentRequest.Url = url;
            this.currentRequest.QueryParameters = this.queryParamPanel.Value;
            this.currentRequest.Headers = this.headerParamPanel.Value;

            currentRequest = request;

            this.urlTextbox.Text = request.Url;
            this.methodControl.Value = request.Method;
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
            var saveDialog = new SaveRequestDialog(this);
            var result = await saveDialog.ShowAsync();
            if (result != ContentDialogResult.Primary)
            {
                return;
            }

            this.SaveRequest(saveDialog.Text);
        }

        public void SaveRequest(string name)
        {
            var url = this.urlTextbox.Text;
            if (this.currentRequest == null) {
                this.currentRequest = new Request();
            }

            this.currentRequest.Name = name;
            this.currentRequest.Method = this.methodControl.Value;
            this.currentRequest.Url = url;
            this.currentRequest.QueryParameters = this.queryParamPanel.Value;
            this.currentRequest.Headers = this.headerParamPanel.Value;

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
            clearRequest();
            currentRequest = new Request();
        }

        private void clearRequest()
        {
            this.urlTextbox.Text = "";
            this.methodControl.Clear();
            this.queryParamPanel.Value = new List<Parameter>();
        }
    }
}
