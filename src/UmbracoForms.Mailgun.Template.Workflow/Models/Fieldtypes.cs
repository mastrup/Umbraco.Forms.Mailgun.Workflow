using System.ComponentModel;

namespace UmbracoForms.Mailgun.Template.Workflow.Models
{
    /// <summary>
    /// All Field Type Ids
    /// </summary>
    public enum FieldTypes
    {
        [Description("d5c0c390-ae9a-11de-a69e-666455d89593")]
        Checkbox,
        [Description("f8b4c3b8-af28-11de-9dd8-ef5956d89593")]
        Date,
        [Description("0dd29d42-a6a5-11de-a2f2-222256d89593")]
        Dropdown,
        [Description("3f92e01b-29e2-4a30-bf33-9df5580ed52c")]
        ShortAnswer,
        [Description("023f09ac-1445-4bcb-b8fa-ab49f33bd046")]
        LongAnswer,
        [Description("84a17cf8-b711-46a6-9840-0e4a072ad000")]
        FileUpload,
        [Description("fb37bc60-d41e-11de-aeae-37c155d89593")]
        Password,
        [Description("fab43f20-a6bf-11de-a28f-9b5755d89593")]
        MultipleChoice,
        [Description("903df9b0-a78c-11de-9fc1-db7a56d89593")]
        SingleChoice,
        [Description("e3fbf6c4-f46c-495e-aff8-4b3c227b4a98")]
        TitleAndDescription,
        [Description("4a2e8e12-9613-4720-9bcd-f9871262d6ac")]
        Recaptcha,
        [Description("da206cae-1c52-434e-b21a-4a7c198af877")]
        Hidden,
        [Description("a72c9df9-3847-47cf-afb8-b86773fd12cd")]
        DataConsent,
    }
}
