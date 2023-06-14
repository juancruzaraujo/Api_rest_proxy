using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ProxyRest
{
    internal class Connector
    {
        internal async Task<HostResponse> ConnectToHost(DestinyHostParameters destinyHostParameters)
        {
            HostResponse hostResponse = new HostResponse();

            string responseBody = string.Empty;

            using (HttpClientHandler handler = new HttpClientHandler())
            {

                // Ignorar la validación de certificados SSL
                handler.ServerCertificateCustomValidationCallback = IgnoreCertificateValidation;

                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        HttpMethod httpMethod = new HttpMethod(destinyHostParameters.Method.ToUpper());
                        HttpRequestMessage request = new HttpRequestMessage(httpMethod, destinyHostParameters.GetHost);

                        //agrego todos los headers
                        IHeaderDictionary headers = destinyHostParameters.GetHeaders;
                        var enumerator = headers.GetEnumerator();
                        
                        try
                        {
                            while (enumerator.MoveNext())
                            {
                                string key = enumerator.Current.Key;
                                string[] values = enumerator.Current.Value;

                                bool haveAuth = key.IndexOf("Authorization", StringComparison.OrdinalIgnoreCase) >= 0;
                                if (haveAuth)
                                {
                                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(key, enumerator.Current.Value);
                                    string currentValue = enumerator.Current.Value;
                                    int keyIndex = currentValue.IndexOf(' ');
                                    string authType = currentValue.Substring(0, keyIndex);
                                    string auth = currentValue.Substring(keyIndex + 1);
                                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authType, auth);
                                    haveAuth = false;
                                }

                                /*
                                bool haveContent = key.IndexOf("Content", StringComparison.OrdinalIgnoreCase) >= 0;
                                if (!haveContent && !haveAuth)
                                {
                                    foreach (string value in values)
                                    {
                                        //request.Headers.Add(key, value);
                                    }
                                }
                                */
                            }
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        HttpResponseMessage response = null;
                        HttpContent content = new StringContent(destinyHostParameters.GetBody, Encoding.UTF8);
                        
                        switch (destinyHostParameters.Method.ToUpper())
                        {
                            case "GET":
                                response = await client.SendAsync(request);
                                break;

                            case "POST":
                                response = await client.PostAsync(destinyHostParameters.GetHost, content);
                                break;

                            case "PUT":
                                response = await client.PutAsync(destinyHostParameters.GetHost,content);
                                break;

                            case "DELETE":
                                response = await client.DeleteAsync(destinyHostParameters.GetHost);
                                break;
                                
                            default:
                                response = await client.SendAsync(request);
                                break;
                        }
                        hostResponse.SetStatusCode = response.StatusCode;
                        response.EnsureSuccessStatusCode();
                        hostResponse.ResponseBody = await response.Content.ReadAsStringAsync();

                    }
                    catch (HttpRequestException ex)
                    {
                        hostResponse.ExcecutionError = ex.Message;
                    }
                }
            }

            return hostResponse;
        }

        static bool IgnoreCertificateValidation(HttpRequestMessage request, X509Certificate2 certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

    }
}
