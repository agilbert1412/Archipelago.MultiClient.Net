using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Archipelago.MultiClient.Net.Converters
{
    internal class SlotDataConverter : JsonConverter<Dictionary<string, object>>
    {
        public override Dictionary<string, object> ReadJson(
            JsonReader reader,
            Type objectType,
            Dictionary<string, object> existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            var dict = new Dictionary<string, object>();

            foreach (var prop in obj.Properties())
            {
                switch (prop.Value.Type)
                {
                    case JTokenType.Integer:
                        dict[prop.Name] = prop.Value.Value<long>();
                        break;
                    case JTokenType.Float:
                        dict[prop.Name] = prop.Value.Value<double>();
                        break;
                    case JTokenType.String:
                        dict[prop.Name] = prop.Value.Value<string>();
                        break;
                    case JTokenType.Boolean:
                        dict[prop.Name] = prop.Value.Value<bool>();
                        break;
                    case JTokenType.Null:
                        dict[prop.Name] = null;
                        break;
                    default:
                        dict[prop.Name] = prop.Value.ToObject<object>();
                        break;
                }
            }

            return dict;
        }

        public override void WriteJson(
            JsonWriter writer,
            Dictionary<string, object> value,
            JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}