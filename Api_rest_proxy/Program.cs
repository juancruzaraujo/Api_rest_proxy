using System;
using ProxyRest;

namespace Api_rest_proxy 
{
    internal class Program
    {
        const int C_DESTINY_HOST_ARG = 0;
        const int C_HTTP_PORT_ARG = 1;
        const int C_HTTPS_PORT_ARG = 2;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Uso: Api_rest_proxy [host] [http port] [https port]");
                Console.WriteLine("[host] destino al que me voy a conectar");
                Console.WriteLine("[http] puerto http que escucha el proxy");
                Console.WriteLine("[https] puerto https que escucha el proxy");
                Console.WriteLine("uso Api_rest_proxy https://example.com:1789 2019 2020");
                Console.WriteLine("para usar el proxy:");
                Console.WriteLine("https://localhost:2020/path/?variableName=value");
                return;
            }
            
            int httpPort = Convert.ToInt32(args[C_HTTP_PORT_ARG]);
            int httpsPort = Convert.ToInt32(args[C_HTTPS_PORT_ARG]);
            string destinyHost = args[C_DESTINY_HOST_ARG];

            Console.WriteLine("======= START =======");
            Console.WriteLine("HOST DESTINO " + args[C_DESTINY_HOST_ARG]);

            Proxy proxy = new Proxy();  
            proxy.StartService(httpPort, httpsPort, destinyHost).Wait();

            Console.WriteLine("======= END =======");
            
        }
    }
}


