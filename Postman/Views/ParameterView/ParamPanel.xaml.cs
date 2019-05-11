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

namespace Postman
{
    public sealed partial class ParamPanel : UserControl
    {
        private List<Parameter> parameterList;

        public ParamPanel()
        {
            this.InitializeComponent();
            this.parameterList = new List<Parameter>();
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
                    var param = item.Value;
                    if (param == null || param.Name.Trim() == "")
                    {
                        continue;
                    }

                    result.Add(param);
                }

                return result;
            }

            set
            {
                this.parameterList.Clear();
                this.parameterList = value;

                var children = this.panel.Children;
                children.Clear();

                if (value.Count == 0)
                {
                    this.AddItem(0);
                    return;
                }

                foreach(var param in value)
                {
                    ParamItem item = new ParamItem(param);
                    children.Add(item);
                }

                indexItem();
            }
        }

        public void AddItem(uint index)
        {
            var param = new Parameter();
            this.parameterList.Add(param);

            var children = this.panel.Children;
            ParamItem item = new ParamItem(param);

            uint count = Convert.ToUInt32(children.Count);
            if (index == count)
            {
                children.Add(item);
                this.parameterList.Add(param);

            } else {
                children.Insert(Convert.ToInt32(index), item);
                this.parameterList.Insert(Convert.ToInt32(index), param);
            }

            this.indexItem();
        }

        public void DeleteItem(uint index)
        {
            this.parameterList.RemoveAt(Convert.ToInt32(index));
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
