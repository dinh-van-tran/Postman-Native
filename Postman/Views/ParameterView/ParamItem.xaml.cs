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

using DataAccessLibrary;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Postman
{
    public sealed partial class ParamItem : UserControl
    {
        private uint index;
        private WeakReference<ParamPanel> panel;

        private Parameter parameter;

        public uint Index
        {
            get { return index; }
            set { index = value; }
        }

        public ParamPanel Panel
        {
            get {
                ParamPanel result;
                if (panel == null || !panel.TryGetTarget(out result))
                {
                    return null;
                }

                return result;
            }
            set {
                if (panel != null)
                {
                    panel.SetTarget(value);
                }
                else
                {
                    panel = new WeakReference<ParamPanel>(value);
                }
            }
        }

        public Visibility DeleteButtonVisibility
        {
            get { return this.deleteButton.Visibility; }
            set { this.deleteButton.Visibility = value; }
        }

        public Parameter Value
        {
            get {
                return this.parameter;
            }
            set
            {
                this.parameter = value;
                this.DataContext = value;
            }
        }

        public ParamItem(Parameter param)
        {
            this.InitializeComponent();
            this.parameter = param;
            this.DataContext = param;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Panel == null)
            {
                return;
            }

            this.Panel.AddItem(index + 1);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Panel == null)
            {
                return;
            }

            this.Panel.DeleteItem(index);
        }
    }
}
