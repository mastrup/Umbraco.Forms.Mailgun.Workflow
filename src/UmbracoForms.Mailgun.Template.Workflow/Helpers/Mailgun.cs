using RestSharp;
using RestSharp.Authenticators;
using System;
using Umbraco.Forms.Core;

namespace UmbracoForms.Mailgun.Template.Workflow.Helpers
{
    internal static class MailgunConfiguration
    {
        public static string ApiKey { get { return GetConfigSetting("MailgunApiKey"); } }
        public static string Domain { get { return GetConfigSetting("MailgunDomain"); } }

        private static string GetConfigSetting(string settingName)
        {
            var setting = Configuration.GetSetting(settingName);
            return setting ?? string.Empty;
        }
    }

    public class Mailgun
    {
        public static RestClient Client()
        {
            RestClient client = new RestClient
            {
                BaseUrl = new Uri("https://api.mailgun.net/v3"),
                Authenticator = new HttpBasicAuthenticator("api", MailgunConfiguration.ApiKey)
            };

            return client;
        }

        public static IRestResponse CheckConnection()
        {
            RestClient client = Client();
            RestRequest request = new RestRequest();
            request.AddParameter("domain", MailgunConfiguration.Domain, ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", $"Umbraco Forms Test <mailgun@{MailgunConfiguration.Domain}>");
            request.AddParameter("to", "bar@example.com");
            request.AddParameter("subject", "Connection check from Umbraco");
            request.AddParameter("text", "Testing some Mailgun awesomness with Umbraco Forms!");
            request.AddParameter("o:testmode", "true");
            request.Method = Method.POST;
            var response = client.Execute(request);

            return response;
        }
    }
}
