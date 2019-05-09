using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postman
{
    public class Param
    {
        private string name { get; set; }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string value;
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public Param()
        {

        }

        public Param(string value, string name)
        {
            this.name = name;
            this.value = value;
        }
    }
}
