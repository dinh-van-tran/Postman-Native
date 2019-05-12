using Windows.UI.Xaml.Controls;

namespace Postman.Views.RequestView
{
    public sealed partial class SaveRequestDialog : ContentDialog
    {

        public SaveRequestDialog(MainPage mainPage)
        {
            this.InitializeComponent();
        }

        public string Text
        {
            get { return this.nameTextBox.Text; }
            set { this.nameTextBox.Text = value; }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
