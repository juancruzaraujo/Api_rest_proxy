using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProxyRest
{
    internal class HostResponse
    {
        //la respuesta del host
        string _responseBody;
        string _excecutionError;
        HttpStatusCode _statusCode;
        string _contentType;

        internal HostResponse()
        {
            _responseBody = "";
            _excecutionError = "";
            _contentType = "";
        }

        internal string ResponseBody
        {
            get
            {
                return _responseBody;
            }
            set
            {
                _responseBody = value;  
            }
        }

        internal string ExcecutionError
        {
            get
            {
                return _excecutionError + "";
            }
            set
            {
                _excecutionError = value;
            }
        }

        internal HttpStatusCode SetStatusCode
        {
            set
            {
                _statusCode = value;
            }
        }

        internal HttpStatusCode GetSatatusCode
        {
            get
            {
                return _statusCode;
            }
        }

        internal string SetContentType
        {
            set
            {
                _contentType = value;
            }
        }

        internal string GetContentType
        {
            get
            {
                return _contentType; 
            }
        }

    }
}
