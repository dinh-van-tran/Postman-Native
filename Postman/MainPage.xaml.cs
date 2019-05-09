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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Postman
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private int id = -1;

        public MainPage()
        {
            this.InitializeComponent();
            DataAccess.InitializeDatabase();
        }

        private void loadData()
        {
            List<Request> requests = DataAccess.getAllRequest();
            if (requests.Count == 0)
            {
                return;
            }

            var request = requests[requests.Count - 1];

            this.urlTextbox.Text = request.Url;

            switch (request.Method)
            {
                case "GET":
                    this.methodComboBox.SelectedIndex = 0;
                    break;
                case "POST":
                    this.methodComboBox.SelectedIndex = 1;
                    break;
                case "PUT":
                    this.methodComboBox.SelectedIndex = 2;
                    break;
                case "DELETE":
                    this.methodComboBox.SelectedIndex = 3;
                    break;
                default:
                    this.methodComboBox.SelectedIndex = 0;
                    break;
            }

            this.queryParamPanel.Value = request.QueryParameters;
            this.id = request.Id;
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.saveData();
        }

        private void saveData()
        {
            var url = this.urlTextbox.Text;
            string method = "GET";

            switch (methodComboBox.SelectedIndex)
            {
                case 0:
                    method = "GET";
                    break;
                case 1:
                    method = "POST";
                    break;
                case 2:
                    method = "PUT";
                    break;
                case 3:
                    method = "DELETE";
                    break;
                default:
                    method = "GET";
                    break;
            }

            var request = new Request(this.id, method, url);
            request.QueryParameters = this.queryParamPanel.Value;

            DataAccess.saveRequest(request);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.loadData();
        }
    }
}
