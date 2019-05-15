using Windows.UI.Xaml.Controls;

using DataAccessLibrary;

namespace Postman
{
    public sealed partial class BodyControl : UserControl
    {
        private Request request;
        public Request Value
        {
            get { return request; }
            set {
                request = value;
                this.bodyText.Text = value.TextParameter;
                this.bodyParamPanel.Value = value.FormParameters;

                switch (value.BodyParameterType)
                {
                    case "TEXT":
                        this.pivot.SelectedIndex = 1;
                        break;
                    default:
                        this.pivot.SelectedIndex = 0;
                        break;
                }
            }
        }

        private string TextParameter
        {
            get { return this.request.TextParameter; }
            set { this.request.TextParameter = value; }
        }

        public BodyControl()
        {
            this.InitializeComponent();
            this.request = new Request();
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (this.pivot.SelectedIndex)
            {
                case 0:
                    this.request.BodyParameterType = "FORM";
                    break;
                default:
                    this.request.BodyParameterType = "TEXT";
                    break;
            }
        }

        private void BodyText_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.request.TextParameter = this.bodyText.Text;
        }
    }
}
