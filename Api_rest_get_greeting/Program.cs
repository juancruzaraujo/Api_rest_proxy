using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace Api_rest_get_greeting
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            int httpPort = Convert.ToInt32(args[0]);
            int httpsPort = Convert.ToInt32(args[1]);//httpPort + 1;

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls($"http://0.0.0.0:{httpPort};https://0.0.0.0:{httpsPort}")
                .Configure(app =>
                {
                    app.Run(async context =>
                    {
                        if (context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
                        {

                            string message;
                            string headerUsername = context.Request.Headers["username"];
                            string username = context.Request.Query["username"];


                            message = "Hola Mundo!";
                            if (!string.IsNullOrEmpty(headerUsername))
                            {
                                message = $"Hola {headerUsername}";
                            }
                            
                            if (!string.IsNullOrEmpty(username))
                            {
                                message = $"Hola {username}";
                            }

                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync($"{{\"message\":\"{message}\"}}", Encoding.UTF8);
                        }
                        else
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                        }
                    });
                })
                .Build();

            await host.RunAsync();
        }
    }
}
