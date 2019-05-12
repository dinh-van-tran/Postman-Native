using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236
using DataAccessLibrary;
namespace Postman
{
    public sealed partial class RequestParamControl : UserControl
    {
        private Request request;

        public Request Request
        {
            get { return request; }
            set {
                request = value;
                this.headerControl.Value = request.Headers;
                this.queryParamControl.Value = request.QueryParameters;
                this.bodyControl.Value = request;
            }
        }

        public RequestParamControl()
        {
            this.InitializeComponent();
        }
    }
}
