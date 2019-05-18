using DataAccessLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Postman.Services
{
    public class RestClient
    {
        public static async Task<Response> SendRequestAsync(Request request)
        {
            var variableList = DataAccess.GetAllVariables();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            HttpClient client = new HttpClient();
            var response = await client.SendAsync(buildRequest(request, variableList));

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

        private static HttpRequestMessage buildRequest(Request request, List<Variable> variables)
        {
            var httpRequest = new HttpRequestMessage();

            httpRequest.RequestUri = buildUri(request, variables);
            httpRequest.Method = getMethod(request.Method);
            setHeaders(request, httpRequest.Headers, variables);
            httpRequest.Content = buildContent(request, variables);

            return httpRequest;
        }

        private static Uri buildUri(Request request, List<Variable> variables)
        {
            UriBuilder builder = new UriBuilder(replaceVariableValue(request.Url, variables));

            builder.Query += "?";
            foreach (var param in request.QueryParameters)
            {
                if (param.Name.Trim() == "")
                {
                    continue;
                }

                string name = replaceVariableValue(param.Name, variables);
                string value = replaceVariableValue(param.Value, variables);

                builder.Query += string.Format("{0}={1}&", name, value);
            }
            builder.Query = builder.Query.Substring(1);

            return builder.Uri;
        }

        
        private static string replaceVariableValue(string param, List<Variable> variables)
        {
            if (!checkHasVariable(param))
            {
                return param;
            }

            foreach (var variable in variables)
            {
                param = param.Replace("{{" + variable.Name + "}}", variable.Value);
            }

            return param;
        }

        private static bool checkHasVariable(string param)
        {
            var match = Regex.Matches(param, @"{{\w+}}");
            if (match.Count != 0)
            {
                return true;
            }

            return false;
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

        private static HttpContent buildContent(Request request, List<Variable> variables)
        {
            if (request.BodyParameterType == "FORM")
            {
                return buildFormContent(request, variables);
            }

            return buildTextContent(request, variables);
        }

        private static HttpContent buildFormContent(Request request, List<Variable> variables)
        {
            var keyValues = new List<KeyValuePair<string, string>>();
            foreach (var parameter in request.FormParameters)
            {
                if (parameter == null)
                {
                    continue;
                }

                string name = replaceVariableValue(parameter.Name, variables);
                if (name == null || name.Trim().Length == 0)
                {
                    continue;
                }

                string value = replaceVariableValue(parameter.Value, variables);

                keyValues.Add(new KeyValuePair<string, string>(name, value));
            }
            return new FormUrlEncodedContent(keyValues);
        }

        private static HttpContent buildTextContent(Request request, List<Variable> variables)
        {
            var value = replaceVariableValue(request.TextParameter, variables);
            return new StringContent(value, new UTF8Encoding() , "application/json");
        }

        private static void setHeaders(Request request, HttpRequestHeaders requestHeaders, List<Variable> variables)
        {
            foreach (var header in request.Headers)
            {
                string name = replaceVariableValue(header.Name, variables);
                if (name.Trim() == "")
                {
                    continue;
                }

                string value = replaceVariableValue(header.Value, variables);

                if (name.ToLower().Equals("content-type"))
                {
                    requestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue(value.ToLower())
                    );
                    continue;
                }

                requestHeaders.Add(name, value);
            }
        }
    }
}
