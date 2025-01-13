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
            public const string ForgetPasswordEndpoint = AuthenticationEndpoint + "/forget-password";
            public const string LoginEndpoint = AuthenticationEndpoint + "/login";
            public const string GoogleLoginEndpoint = AuthenticationEndpoint + "/login-google";
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

            public const string SaleCampaignProductsEndpoint = SaleCampaignEndpoint + "/products";
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

            public const string TransactionsVNPaymentEndpoint = TransactionsEndpoint + "/vnpay";
            public const string TransactionsVNPaymentReturnEndpoint = TransactionsEndpoint + "/vnpay-return";

            public const string TransactionsPaypalEndpoint = TransactionsEndpoint + "/paypal";
            public const string TransactionsPaypalCaptureEndpoint = TransactionsPaypalEndpoint + "/capture";
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

        // New endpoints for the entities I've worked on
        public static class Account
        {
            public const string AccountsEndpoint = ApiEndpoint + "/accounts";
            public const string AccountsEmailEndpoint = AccountsEndpoint +"/{email}/info";
            public const string AccountEndpoint = AccountsEndpoint + "/{id}";
            public const string SoftDeleteAccountEndpoint = AccountsEndpoint + "/soft-delete/{id}";
        }

        public static class Product
        {
            public const string ProductsEndpoint = ApiEndpoint + "/products";
            public const string ProductEndpoint = ProductsEndpoint + "/{id}";
            public const string ProductReviewsEndpoint = ProductEndpoint + "/reviews";
            public const string SoftDeleteProductEndpoint = ProductsEndpoint + "/soft-delete/{id}";
        }

        public static class ProductCategory
        {
            public const string ProductCategoriesEndpoint = ApiEndpoint + "/product-categories";
            public const string ProductCategoryEndpoint = ProductCategoriesEndpoint + "/{id}";
        }

        public static class Order
        {
            public const string OrdersEndpoint = ApiEndpoint + "/orders";
            public const string OrderEndpoint = OrdersEndpoint + "/{id}";
            public const string CustomerOrderEndpoint = OrdersEndpoint + "/customer/{customerId}";
            public const string MerchantOrdersEndpoint = OrdersEndpoint + "/merchant/{id}";
        }

        public static class Merchant
        {
            public const string MerchantsEndpoint = ApiEndpoint + "/merchants";
            public const string UnverifiedMerchantsEndpoint = MerchantsEndpoint + "/unverified";
            public const string MerchantEndpoint = MerchantsEndpoint + "/{id}";
            public const string MerchantVerificationEndpoint = MerchantEndpoint + "/verification";
        }

        public static class Design
        {
            public const string DesignsEndpoint = ApiEndpoint + "/designs";
            public const string DesignEndpoint = DesignsEndpoint + "/{id}";
        }

        public static class ChatSession
        {
            public const string ChatSessionsEndpoint = ApiEndpoint + "/chat-sessions";
            public const string MerchantChatSessionsEndpoint = ChatSessionsEndpoint + "/merchant/{id}";
            public const string ManagerChatSessionsEndpoint = ChatSessionsEndpoint + "/manager";
            public const string ChatSessionEndpoint = ChatSessionsEndpoint + "/{id}";

            public const string ChatMessageEndpoint = ChatSessionEndpoint + "/messages";
        }

        public static class Floor
        {
            public const string FloorsEndpoint = ApiEndpoint + "/floors";
            public const string FloorEndpoint = FloorsEndpoint + "/{id}";
        }
    }
}
