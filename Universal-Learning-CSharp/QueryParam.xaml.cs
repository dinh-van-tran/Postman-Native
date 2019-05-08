using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Universal_Learning_CSharp
{
    public sealed partial class QueryParam : UserControl
    {
        public QueryParam()
        {
            this.InitializeComponent();
        }

        public string QueryName {
            get { return this.Name.Text; }
            set { this.Name.Text = value; }
        }

        public string QueryValue
        {
            get { return this.Value.Text; }
            set { this.Value.Text = value; }
        }

        public string QueryString
        {
            get
            {
                if (this.Name.Text == "")
                {
                    return "";
                }

                return string.Format("{0}={1}", this.Name.Text, this.Value.Text);
            }
        }
    }
}
