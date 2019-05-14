using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;
using Umbraco.Forms.Core.Enums;
using Umbraco.Forms.Core.Persistence.Dtos;
using UmbracoForms.Mailgun.Workflow.Helpers;
using UmbracoForms.Mailgun.Workflow.Models;

namespace UmbracoForms.Mailgun.Workflow.Workflows
{
    //https://github.com/Matthew-Wise/umbraco-forms-campaign-monitor/blob/master/src/Mw.UmbFormsCampaignMonitor/WorkFlows/PushToMarketing.cs
    public class SendMailgunTemplate : WorkflowType
    {
        public SendMailgunTemplate()
        {
            Name = "Send email with template (Mailgun)";
            Icon = "icon-message";
            Description = "Send the result of the form to an email address/addresses using a Mailgun template";
            Id = new Guid("bcd055d2-ad50-4ab3-b83d-749e408a7995");
        }

        [Setting("Email", Description = "Enter the receiver email")]
        public string ReceiverAddress { get; set; }

        [Setting("Sender email", Description = "Enter the sender email (if blank it will use the settings from /config/umbracosettings.config)")]
        public string FromAddress { get; set; }

        [Setting("Reply-to", Description = "If needed, set an address to which replies should be sent, instead of 'From'")]
        public string ReplyToAddress { get; set; }

        [Setting("Subject", Description = "Enter the subject")]
        public string Subject { get; set; }

        [Setting("Mailgun template", Description = "Pick the Mailgun template you wish to use", View = "~/App_Plugins/Oerskov.UmbracoFormsMailgun/templatePicker.html")]
        public string Template { get; set; }

        public override WorkflowExecutionStatus Execute(Record record, RecordEventArgs e)
        {
            try
            {
                if (ValidateSettings().Any() || string.IsNullOrEmpty(MailgunConfiguration.ApiKey) || string.IsNullOrEmpty(MailgunConfiguration.Domain))
                {
                    return WorkflowExecutionStatus.NotConfigured;
                }

                RestClient client = Helpers.Mailgun.Client();
                RestRequest request = new RestRequest();
                request.AddParameter("domain", MailgunConfiguration.Domain, ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";

                //FromAddress = !string.IsNullOrEmpty(FromAddress) ? FromAddress : Umbraco.Core.Configuration.UmbracoSettings.IContentSection.NotificationEmailAddress;
                //UmbracoConfig.For.UmbracoSettings().Content.NotificationEmailAddress)
                request.AddParameter("from", FromAddress);


                if (!string.IsNullOrEmpty(ReplyToAddress))
                {
                    request.AddParameter("h:Reply-To", ReplyToAddress);
                }
                request.AddParameter("to", ReceiverAddress);
                request.AddParameter("subject", Subject);
                request.AddParameter("template", Template);

                var messageVariables = new Dictionary<string, object>();

                foreach (var field in record.RecordFields.Where(x => !x.Value.Field.ContainsSensitiveData))
                {
                    var fieldValue = string.Empty;
                    switch (FieldtypeHelper.GetValueFromDescription<FieldTypes>(field.Value.Field.FieldTypeId))
                    {
                        case FieldTypes.FileUpload:
                            var context = HttpContext.Current.Request.Url;
                            fieldValue = field.Value.ValuesAsString(false);
                            if (!string.IsNullOrEmpty(fieldValue))
                            {
                                fieldValue = context.Scheme + "://" + context.Host + fieldValue;
                                messageVariables.Add(field.Value.Alias, fieldValue);
                            }
                            break;
                        case FieldTypes.MultipleChoice:
                            messageVariables.Add(field.Value.Alias, field.Value.Values);
                            break;
                        case FieldTypes.DataConsent:
                            fieldValue = field.Value.ValuesAsString(false).ToLower();
                            messageVariables.Add(field.Value.Alias, fieldValue);
                            break;
                        case FieldTypes.Checkbox:
                            fieldValue = field.Value.ValuesAsString(false).ToLower();
                            messageVariables.Add(field.Value.Alias, fieldValue);
                            break;
                        default:
                            fieldValue = field.Value.ValuesAsString(false);
                            messageVariables.Add(field.Value.Alias, fieldValue);
                            break;
                    }   
                }

                request.AddParameter("h:X-Mailgun-Variables", JsonConvert.SerializeObject(messageVariables));
                request.Method = Method.POST;

                client.Execute(request);

                return WorkflowExecutionStatus.Completed;
            }
            catch (Exception ex)
            {
                Serilog.Core.Logger.None.Error("Mailgun workflow failed", ex);
                //Logger.Error<SendMailgunTemplate>("Failed to send users record to marketing", ex);
                return WorkflowExecutionStatus.Failed;
            }
        }

        public override List<Exception> ValidateSettings()
        {
            var errors = new List<Exception>();
            if (string.IsNullOrWhiteSpace(ReceiverAddress))
            {
                errors.Add(new Exception("'Email' has not been set"));
            }
            if (string.IsNullOrWhiteSpace(FromAddress))
            {
                errors.Add(new Exception("'Sender email' has not been set"));
            }
            if (string.IsNullOrWhiteSpace(Subject))
            {
                errors.Add(new Exception("'Subject' has not been set"));
            }
            if (string.IsNullOrWhiteSpace(Template))
            {
                errors.Add(new Exception("'Mailgun template' has not been set"));
            }

            if (string.IsNullOrEmpty(MailgunConfiguration.ApiKey))
            {
                errors.Add(new Exception("'Mailgun API key' has not been set in the config"));
            }

            if (string.IsNullOrEmpty(MailgunConfiguration.Domain))
            {
                errors.Add(new Exception("'Mailgun Domain' has not been set in the config"));
            }

            return errors;
        }
    }
}