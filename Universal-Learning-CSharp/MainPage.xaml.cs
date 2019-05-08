using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Universal_Learning_CSharp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void sendButtonClick(object sender, RoutedEventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            //foreach (var child in this.queryParamPanel.Children)
            //{
            //    var param = (ParamItem)child;
            //    var queryString = param.QueryString;
            //    if (queryString == "")
            //    {
            //        continue;
            //    }

            //    builder.Append(param.QueryString).Append("&");
            //}
            if (builder.Length > 0)
            {
                builder.Length--;
                builder.Insert(0, "?");
            }
            
            //string result = await this.sendRequest(this.urlTextbox.Text, "");

            this.responseBodyTextBox.Text = builder.ToString();
        }

        private async Task<string> sendRequest(string url, string param)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);

            // Add an Accept header for JSON format.
            //client.DefaultRequestHeaders.Accept.Add(
            //new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(param).Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                return await response.Content.ReadAsStringAsync();  //Make sure to add a reference to System.Net.Http.Formatting.dll
            }
            else
            {
                //Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return "Error";
            }

            //Make any other calls using HttpClient here.

            //Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
            //client.Dispose();
        }

        private void AddQueryParamButton_Click(object sender, RoutedEventArgs e)
        {
            this.queryParamPanel.Children.Add(new ParamItem());
        }
    }
}
