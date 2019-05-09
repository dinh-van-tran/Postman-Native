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

namespace Postman.Views
{
    public sealed partial class MethodView : UserControl
    {
        public string Value
        {
            get
            {
                switch (this.comboBox.SelectedIndex)
                {
                    case 0:
                        return "GET";
                    case 1:
                        return "POST";
                    case 2:
                        return "PUT";
                    case 3:
                        return "DELETE";
                    default:
                        return "GET";
                }
            }

            set
            {
                switch (value)
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

        public void Clear()
        {
            this.comboBox.SelectedIndex = 0;
        }
    }
}
