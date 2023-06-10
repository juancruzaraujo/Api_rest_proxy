using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyRest
{
    internal class DestinyHostParameters
    {
        string _host;
        IHeaderDictionary _headers;
        string _body;
        string _method;
        Stream _contentBody;

        /*internal DestinyHostParameters()
        {

        }*/

        internal string GetHost
        {
            get
            {
                return _host;
            }
        }
        internal string SetHost
        {
            set
            {
                _host = value;
            }
        }

        internal IHeaderDictionary GetHeaders
        {
            get
            {
                return _headers;
            }
        }

        internal IHeaderDictionary SetHeaders
        {
            set
            {
                _headers = value;
            }

        }

        internal Stream SetContentBody
        {
            set
            {
                _contentBody = value; 
            }
        }

        internal Stream GetContentBody
        {
            get
            {
                return _contentBody;
            }
        }


        internal string SetBody
        {
            set
            {
                _body = value;
            }

        }

        internal string GetBody
        {
            get 
            {
                StreamReader reader = new StreamReader(_contentBody);
                _body = reader.ReadToEnd();
                return _body; 
            }
        }
        
        internal string SetMethod
        {
            set
            {
                _method = value;
            }
        }

        internal string Method
        {
            get
            {
                return _method;
            }
        }

    }
}

