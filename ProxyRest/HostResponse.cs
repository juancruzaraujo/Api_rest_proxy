using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyRest
{
    internal class HostResponse
    {
        //la respuesta del host
        string _responseBody;
        string _excecutionError;
        
        internal HostResponse()
        {
            _responseBody = "";
            _excecutionError = "";
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

    }
}
