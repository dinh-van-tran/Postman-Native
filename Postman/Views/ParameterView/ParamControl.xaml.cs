using DataAccessLibrary;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Postman
{
    public sealed partial class ParamControl : UserControl
    {
        private List<Variable> variableList;

        public ParamControl()
        {
            this.InitializeComponent();
            this.variableList = new List<Variable>();
            this.AddItem(0);
        }


        public List<Variable> Value
        {
            get
            {
                List<Variable> result = new List<Variable>();
                var children = this.panel.Children;
                for (int i = 0; i < children.Count; i++)
                {
                    var item = (ParamItem)children[i];
                    var variable = item.Value;
                    if (variable == null || variable.Name.Trim() == "")
                    {
                        continue;
                    }

                    result.Add(variable);
                }

                return result;
            }

            set
            {

                var children = this.panel.Children;
                children.Clear();

                this.variableList = value;

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
            var children = this.panel.Children;

            var param = new Variable();
            var item = new ParamItem(param);

            uint count = Convert.ToUInt32(children.Count);
            if (index == count)
            {
                children.Add(item);
                this.variableList.Add(param);

            } else {
                children.Insert(Convert.ToInt32(index), item);
                this.variableList.Insert(Convert.ToInt32(index), param);
            }

            this.indexItem();
        }

        public void DeleteItem(uint index)
        {
            this.variableList.RemoveAt(Convert.ToInt32(index));
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
