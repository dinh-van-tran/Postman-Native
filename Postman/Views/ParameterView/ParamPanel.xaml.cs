using DataAccessLibrary;
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

namespace Postman
{
    public sealed partial class ParamPanel : UserControl
    {
        public ParamPanel()
        {
            this.InitializeComponent();
            this.AddItem(0);
        }

        public List<Parameter> Value
        {
            get
            {
                List<Parameter> result = new List<Parameter>();
                var children = this.panel.Children;
                for (int i = 0; i < children.Count; i++)
                {
                    var item = (ParamItem)children[i];
                    result.Add(item.Value);
                }

                return result;
            }

            set
            {
                var children = this.panel.Children;
                children.Clear();
                if (value.Count == 0)
                {
                    this.AddItem(0);
                    return;
                }

                foreach(var param in value)
                {
                    ParamItem item = new ParamItem();
                    item.Value = param;
                    children.Add(item);
                }

                indexItem();
            }
        }

        public void AddItem(uint index)
        {
            var children = this.panel.Children;
            ParamItem item = new ParamItem();
            children.Add(item);

            uint count = Convert.ToUInt32(children.Count);
            if (index != count - 1)
            {
                children.Move(count - 1, index);
            }

            this.indexItem();
        }

        public void DeleteItem(uint index)
        {
            var item = (ParamItem)this.panel.Children[Convert.ToInt32(index)];
            this.panel.Children.RemoveAt(Convert.ToInt32(index));
            this.indexItem();
        }

        private void indexItem()
        {
            var children = this.panel.Children;
            for (int i = 0; i < children.Count; i++)
            {
                var temp = (ParamItem)children[i];
                if (temp.Panel == null)
                {
                    temp.Panel = this;
                }
                temp.Index = Convert.ToUInt32(i);
                temp.DeleteButtonVisibility = Visibility.Visible;
            }

            if (children.Count == 1)
            {
                var temp = (ParamItem)children[0];
                temp.DeleteButtonVisibility = Visibility.Collapsed;
            }
        }
    }
}
