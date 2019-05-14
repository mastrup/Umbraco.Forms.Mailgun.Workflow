using Newtonsoft.Json;
using System.Collections.Generic;

namespace UmbracoForms.Mailgun.Template.Workflow.Models
{
    public class Templates
    {
        [JsonProperty("items")]
        public List<Template> Items { get; set; }
        [JsonProperty("paging")]
        public Paging Paging { get; set; }
    }

    public class Paging
    {
        [JsonProperty("first")]
        public string First { get; set; }
        [JsonProperty("last")]
        public string Last { get; set; }
        [JsonProperty("next")]
        public string Next { get; set; }
        [JsonProperty("previous")]
        public string Previous { get; set; }
    }

    public class Template
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }
    }
}
