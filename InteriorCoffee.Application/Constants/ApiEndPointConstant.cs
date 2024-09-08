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

        public static class Review
        {
            public const string ReviewsEndpoint = ApiEndpoint + "/reviews";
            public const string ReviewEndpoint = ReviewsEndpoint + "/{id}";
        }

        public static class Role
        {
            public const string RolesEndpoint = ApiEndpoint + "/roles";
            public const string RoleEndpoint = RolesEndpoint + "/{id}";
        }

        public static class SaleCampaign
        {
            public const string SaleCampaignsEndpoint = ApiEndpoint + "/sale-campaigns";
            public const string SaleCampaignEndpoint = SaleCampaignsEndpoint + "/{id}";
        }

        public static class Style
        {
            public const string StylesEndpoint = ApiEndpoint + "/styles";
            public const string StyleEndpoint = StylesEndpoint + "/{id}";
        }

        public static class Template
        {
            public const string TemplatesEndpoint = ApiEndpoint + "/templates";
            public const string TemplateEndpoint = TemplatesEndpoint + "/{id}";
        }

        public static class Transaction
        {
            public const string TransactionsEndpoint = ApiEndpoint + "/transactions";
            public const string TransactionEndpoint = TransactionsEndpoint + "/{id}";
        }

        public static class Voucher
        {
            public const string VouchersEndpoint = ApiEndpoint + "/vouchers";
            public const string VoucherEndpoint = VouchersEndpoint + "/{id}";
        }

        public static class VoucherType
        {
            public const string VoucherTypesEndpoint = ApiEndpoint + "/voucher-types";
            public const string VoucherTypeEndpoint = VoucherTypesEndpoint + "/{id}";
        }
    }
}
