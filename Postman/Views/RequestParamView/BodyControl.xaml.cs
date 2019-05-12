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
                this.DataContext = value;
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

        public BodyControl()
        {
            this.InitializeComponent();
            this.request = new Request();
            this.DataContext = this.request;
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
    }
}
