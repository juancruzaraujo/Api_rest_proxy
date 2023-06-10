using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ProxyRest
{
    public class Proxy
    {
        public async Task StartService(int httpPort, int httpsPort, string destinyHost)
        {
            
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls($"http://0.0.0.0:{httpPort};https://0.0.0.0:{httpsPort}")
                .Configure(app =>
                {

                    DestinyHostParameters destinyHostParameters = new DestinyHostParameters();

                    app.Run(async context =>
                    {
                        string queryStr = "" + context.Request.QueryString;
                        string path = "" + context.Request.Path;

                        destinyHostParameters.SetMethod = context.Request.Method;
                        destinyHostParameters.SetHeaders = context.Request.Headers;
                        destinyHostParameters.SetHost = destinyHost + path + queryStr;
                        destinyHostParameters.SetContentBody = context.Request.Body;

                        Console.WriteLine(destinyHostParameters.GetHost);

                        Connector connector = new Connector();
                        HostResponse hostResponse = await connector.ConnectToHost(destinyHostParameters); //llamo al api en cuestion

                        string response = "";
                        if (hostResponse.ExcecutionError !="")
                        {
                            response = hostResponse.ExcecutionError;
                        }
                        if (hostResponse.ResponseBody != "")
                        {
                            response = hostResponse.ResponseBody;
                        }

                        await context.Response.WriteAsync(response, Encoding.UTF8);
                        
                    });
                })
                .Build();
                
            await host.RunAsync();
        }
    }
}