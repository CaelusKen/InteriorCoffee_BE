using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Constants
{
    public static class ApiEndPointConstant
    {
        public const string RootEndPoint = "/api";
        public const string ApiVersion = "/v1";
        public const string ApiEndpoint = RootEndPoint + ApiVersion;

        public static class Authentication
        {
            public const string AuthenticationEndpoint = ApiEndpoint + "/auth";
            public const string LoginEndpoint = AuthenticationEndpoint + "login";
            public const string RegisterEndpoint = AuthenticationEndpoint + "/register";
            public const string MerchantRegisterEndpoint = AuthenticationEndpoint + "/merchant/register";
        }
    }
}
