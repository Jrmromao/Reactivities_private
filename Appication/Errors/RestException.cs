using System;
using System.Net;

namespace Appication.Errors {
    public class RestException : Exception {
       
        public RestException (HttpStatusCode code, object error = null) {
            
            
            Code = code;
            Error = error;
        }

        public HttpStatusCode Code { get; }
        public object Error { get; }
    }
}