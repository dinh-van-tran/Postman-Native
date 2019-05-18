using System.Collections.Generic;

namespace DataAccessLibrary
{
    public class Request
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string method;
        public string Method
        {
            get { return method; }
            set { method = value; }
        }

        private string url;
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        private List<Variable> headers;
        public List<Variable> Headers
        {
            get { return headers; }
            set { headers = value; }
        }

        private List<Variable> queryParameters;
        public List<Variable> QueryParameters
        {
            get { return queryParameters; }
            set { queryParameters = value; }
        }

        private string bodyParameterType;
        public string BodyParameterType
        {
            get { return bodyParameterType; }
            set { bodyParameterType = value; }
        }

        private List<Variable> formParameters;
        public List<Variable> FormParameters
        {
            get { return formParameters; }
            set { formParameters = value; }
        }

        private string textParameter;
        public string TextParameter
        {
            get { return textParameter; }
            set { textParameter = value; }
        }

        public Request()
        {
            this.id = -1;
            this.name = "";
            this.method = "GET";
            this.url = "";
            this.queryParameters = new List<Variable>();
            this.headers = new List<Variable>();
            this.bodyParameterType = "TEXT";
            this.textParameter = "";
            this.formParameters = new List<Variable>();
        }

        public Request(string name, string method, string url)
        {
            this.id = -1;
            this.name = name;
            this.method = method;
            this.url = url;
            this.queryParameters = new List<Variable>();
            this.headers = new List<Variable>();
            this.bodyParameterType = "TEXT";
            this.textParameter = "";
            this.formParameters = new List<Variable>();
        }

        public Request(int id, string name, string method, string url)
        {
            this.id = id;
            this.name = name;
            this.method = method;
            this.url = url;
            this.queryParameters = new List<Variable>();
            this.headers = new List<Variable>();
            this.bodyParameterType = "TEXT";
            this.textParameter = "";
            this.formParameters = new List<Variable>();
        }

        public Request(int id, string name, string method, string url, string bodyParameterType, string textParameter)
        {
            this.id = id;
            this.name = name;
            this.method = method;
            this.url = url;
            this.queryParameters = new List<Variable>();
            this.headers = new List<Variable>();
            this.bodyParameterType = bodyParameterType;
            this.textParameter = textParameter;
            this.formParameters = new List<Variable>();
        }
    }
}