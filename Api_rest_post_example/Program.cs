using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Api_rest_post_example
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            int httpPort = Convert.ToInt32(args[0]);
            int httpsPort = Convert.ToInt32(args[1]);

            try
            {

                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseUrls($"http://0.0.0.0:{httpPort};https://0.0.0.0:{httpsPort}")
                    .Configure(app =>
                    {

                        app.Use(async (context, next) =>
                        {
                            string authHeader = context.Request.Headers["Authorization"] + "";

                            if (authHeader != null && authHeader.StartsWith("Basic"))
                            {
                            // Extraer las credenciales del encabezado de autenticación
                            string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                                Encoding encoding = Encoding.GetEncoding("UTF-8");
                                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                                int separatorIndex = usernamePassword.IndexOf(':');
                                string username = usernamePassword.Substring(0, separatorIndex);
                                string password = usernamePassword.Substring(separatorIndex + 1);

                                if (username == "usuario" && password == "1234")
                                {
                                    await next.Invoke();
                                }
                                else
                                {
                                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                    Console.WriteLine(HttpStatusCode.Unauthorized);
                                    return;
                                }
                            }
                            else if (authHeader != null && authHeader.StartsWith("Bearer"))
                            {
                                // Extraer el token Bearer
                                string token = authHeader.Substring("Bearer ".Length).Trim();

                                // Validar el token aquí (por ejemplo, verificar si es válido y corresponde a un usuario autenticado)

                                if (TokenIsValid(token))
                                {
                                    await next.Invoke();
                                }
                                else
                                {
                                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                    return;
                                }
                            }
                            else
                            {
                            // No se proporcionaron credenciales de autenticación
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                Console.WriteLine(HttpStatusCode.Unauthorized);
                                return;
                            }
                        });

                        app.Run(async context =>
                        {

                            if (context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
                            {
                            // Leer el JSON del cuerpo de la solicitud
                                using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                                {
                                    var json = await reader.ReadToEndAsync();
                                    var response = new { Message = "ERROR" };
                                    // Deserializar el JSON en un objeto
                                    var data = JsonConvert.DeserializeObject<UserData>(json);

                                    if (data != null)
                                    {
                                        response = new { Message = "OK" };
                                    }


                                    var responseJson = JsonConvert.SerializeObject(response);

                                    context.Response.ContentType = "application/json";
                                    await context.Response.WriteAsync(responseJson, Encoding.UTF8);
                                }

                            }
                            else
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                                Console.WriteLine(HttpStatusCode.Unauthorized);
                            }
                        });
                    })
                    .Build();

                await host.RunAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static bool TokenIsValid(string token)
        {
            bool result = false;

            if (token == "saraza")
            {
                result = true;
            }

            return result;
        }
    }
}

