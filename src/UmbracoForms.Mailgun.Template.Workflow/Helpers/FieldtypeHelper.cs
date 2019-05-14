using System;
using System.ComponentModel;

namespace UmbracoForms.Mailgun.Template.Workflow.Helpers
{
    public static class FieldtypeHelper
    {
        public static T GetValueFromDescription<T>(Guid guid)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (new Guid(attribute.Description) == guid)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
            // or return default(T);
        }
    }
}
