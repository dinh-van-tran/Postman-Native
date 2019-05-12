using System.Net;

namespace Postman.Services
{
    public class Response
    {
        private string content;
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        private HttpStatusCode statusCode;
        public HttpStatusCode StatusCode
        {
            get { return statusCode; }
            set { statusCode = value; }
        }

        private long elapsedTime;
        public long ElapsedTime
        {
            get { return elapsedTime; }
            set { elapsedTime = value; }
        }

        public Response()
        {
            this.content = "";
        }
    }
}
