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

        private List<Parameter> headers;
        public List<Parameter> Headers
        {
            get { return headers; }
            set { headers = value; }
        }

        private List<Parameter> queryParameters;
        public List<Parameter> QueryParameters
        {
            get { return queryParameters; }
            set { queryParameters = value; }
        }

        public Request()
        {
            this.id = -1;
        }

        public Request(string name, string method, string url)
        {
            this.id = -1;
            this.name = name;
            this.method = method;
            this.url = url;
        }

        public Request(int id, string name, string method, string url)
        {
            this.id = id;
            this.name = name;
            this.method = method;
            this.url = url;
        }
    }
}