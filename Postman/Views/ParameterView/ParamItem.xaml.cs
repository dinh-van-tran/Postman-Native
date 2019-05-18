using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using DataAccessLibrary;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Postman
{
    public sealed partial class ParamItem : UserControl
    {
        private uint index;
        private WeakReference<ParamControl> panel;

        private Variable parameter;

        public uint Index
        {
            get { return index; }
            set { index = value; }
        }

        public ParamControl Panel
        {
            get {
                ParamControl result;
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
                    panel = new WeakReference<ParamControl>(value);
                }
            }
        }

        public Visibility DeleteButtonVisibility
        {
            get { return this.deleteButton.Visibility; }
            set { this.deleteButton.Visibility = value; }
        }

        public Variable Value
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

        public ParamItem(Variable param)
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
