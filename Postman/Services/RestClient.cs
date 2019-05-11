using DataAccessLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Postman.Services
{
    public class RestClient
    {
        public static async Task<Response> SendRequestAsync(Request request)
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            HttpClient client = new HttpClient();
            var response = await client.SendAsync(buildRequest(request));

            Response result = new Response();
            result.StatusCode = response.StatusCode;
            //if (response.IsSuccessStatusCode)
            //{

            //    if (response.Content.Headers.ContentType.MediaType == "text/html")
            //    {
            //result.Content = await response.Content.ReadAsStringAsync();
            //    } else {
            //        result.Content = await response.Content.ReadAsStringAsync();
            //    }

            //}

            result.Content = await response.Content.ReadAsStringAsync();
            if (response.Content.Headers.ContentType.MediaType == "application/json")
            {
                dynamic parsedJson = JsonConvert.DeserializeObject(result.Content);
                result.Content = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
            }

            stopWatch.Stop();
            result.ElapsedTime = stopWatch.ElapsedMilliseconds;

            return result;
        }

        private static HttpRequestMessage buildRequest(Request request)
        {
            var httpRequest = new HttpRequestMessage();

            httpRequest.RequestUri = buildUri(request);
            httpRequest.Method = getMethod(request.Method);
            setHeaders(request, httpRequest.Headers);
            httpRequest.Content = buildContent(request);


            return httpRequest;
        }

        private static Uri buildUri(Request request)
        {
            UriBuilder builder = new UriBuilder(request.Url);

            builder.Query += "?";
            foreach (var param in request.QueryParameters)
            {
                if (param.Name.Trim() == "")
                {
                    continue;
                }
                builder.Query += string.Format("{0}={1}&", param.Name, param.Value);
            }
            builder.Query = builder.Query.Substring(1);

            return builder.Uri;
        }

        private static HttpMethod getMethod(string method)
        {
            switch (method)
            {
                case "GET":
                    return HttpMethod.Get;
                case "POST":
                    return HttpMethod.Post;
                case "PUT":
                    return HttpMethod.Put;
                case "DELETE":
                    return HttpMethod.Delete;
                default:
                    return HttpMethod.Get;
            }
        }

        private static HttpContent buildContent(Request request)
        {
            var keyValues = new List<KeyValuePair<string, string>>();
            foreach (var parameter in request.FormParameters)
            {
                if (parameter == null || parameter.Name.Trim() == "")
                {
                    continue;
                }

                keyValues.Add(new KeyValuePair<string, string>(parameter.Name, parameter.Value));
            }
            return new FormUrlEncodedContent(keyValues);
        }

        private static void setHeaders(Request request, HttpRequestHeaders requestHeaders)
        {
            foreach (var header in request.Headers)
            {
                if (header.Name.Trim() == "")
                {
                    continue;
                }

                if (header.Name.ToLower().Equals("content-type"))
                {
                    requestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue(header.Value.ToLower())
                    );
                    continue;
                }

                requestHeaders.Add(header.Name, header.Value);
            }
        }
    }
}
