using RestSharp;
using System.Collections.Generic;
using System.Web.Http;
using Umbraco.Web.WebApi;
using UmbracoForms.Mailgun.Template.Workflow.Helpers;
using UmbracoForms.Mailgun.Template.Workflow.Models;

namespace UmbracoForms.Mailgun.Template.Workflow.ApiControllers
{
    //[IsBackOffice]
    public class ListTemplatesApiController : UmbracoAuthorizedApiController
    {
        [HttpGet]
        public List<Models.Template> GetTemplates()
        {
            RestClient client = Helpers.Mailgun.Client();
            RestRequest request = new RestRequest();
            request.AddParameter("domain", MailgunConfiguration.Domain, ParameterType.UrlSegment);
            request.AddParameter("limit", 10);
            request.Resource = "/{domain}/templates";

            var response = client.Execute<Templates>(request);
            
            if(response.Data == null)
            {
                return new List<Models.Template>();
            }

            return response.Data.Items;
        }
    }

}
