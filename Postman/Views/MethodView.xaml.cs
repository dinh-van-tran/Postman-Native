using DataAccessLibrary;
using Windows.UI.Xaml.Controls;

namespace Postman.Views
{
    public sealed partial class MethodView : UserControl
    {
        private Request request;

        public Request Value
        {
            get
            {
                return this.request;
            }

            set
            {
                this.request = value;
                this.DataContext = value;
                switch (value.Method)
                {
                    case "GET":
                        this.comboBox.SelectedIndex = 0;
                        break;
                    case "POST":
                        this.comboBox.SelectedIndex = 1;
                        break;
                    case "PUT":
                        this.comboBox.SelectedIndex = 2;
                        break;
                    case "DELETE":
                        this.comboBox.SelectedIndex = 3;
                        break;
                    default:
                        this.comboBox.SelectedIndex = 0;
                        break;
                }
            }
        }

        public MethodView()
        {
            this.InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.request == null)
            {
                return;
            }

            string method = "GET";
            switch (this.comboBox.SelectedIndex)
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
            this.request.Method = method;
        }
    }
}
