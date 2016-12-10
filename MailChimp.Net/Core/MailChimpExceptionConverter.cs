using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MailChimp.Net.Core
{
    public class MailChimpExceptionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var exception = (MailChimpException)value;

            var obj = new JObject();
            obj.Add("detail", new JValue(exception.Detail));
            obj.Add("title", new JValue(exception.Title));
            obj.Add("type", new JValue(exception.Type));
            obj.Add("status", new JValue(exception.Status));
            obj.Add("instance", new JValue(exception.Instance));
            obj.Add("errors", new JObject(exception.Errors));

            obj.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);

            var detail = obj.GetValue("detail")?.Value<string>();
            var title = obj.GetValue("title")?.Value<string>();
            var type = obj.GetValue("type")?.Value<string>();
            var status = obj.GetValue("status")?.Value<int?>() ?? 0;
            var instance = obj.GetValue("instance")?.Value<string>();
            var errors = obj.GetValue("errors")?.Value<List<MailChimpException.Error>>();

            return new MailChimpException(detail, title, type, status, instance, errors);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(MailChimpException);
        }
    }
}
