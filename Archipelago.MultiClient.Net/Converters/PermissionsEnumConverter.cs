using Archipelago.MultiClient.Net.Enums;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Archipelago.MultiClient.Net.Converters
{
    public class PermissionsEnumConverter : JsonConverter
	{ 
		private void Log(string message)
		{
			var time = DateTime.Now;
			File.AppendAllText("multiclientlog.txt", Environment.NewLine + time.ToLongTimeString() + "." + time.Millisecond + ": " + message);
		}

		public override bool CanConvert(Type objectType)
		{
			Log("PermissionsEnumConverter.CanConvert: " + objectType);
			return objectType == typeof(string)
			       || objectType == typeof(Permissions)
			       || objectType == typeof(int);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Log("PermissionsEnumConverter.ReadJson");
			var value = reader.Value.ToString();
			Log("PermissionsEnumConverter.ReadJson() value: " + value);
			var isInt = int.TryParse(value, out var intValue);

			if (isInt)
			{
				Log("PermissionsEnumConverter.ReadJson() value is an int");
				return (Permissions)intValue;
			}

            var returnValue = Permissions.Disabled;

            if (value.Contains("enabled"))
                returnValue |= Permissions.Enabled;

            if (value.Contains("auto"))
                returnValue |= Permissions.Auto;

            if (value.Contains("goal"))
                returnValue |= Permissions.Goal;

            Log("PermissionsEnumConverter.ReadJson() value is " + returnValue);
			return returnValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			Log("PermissionsEnumConverter.WriteJson: " + value);
			var permissionsValue = (Permissions)value;

            writer.WriteValue((int)permissionsValue);
            Log("PermissionsEnumConverter.WriteJson: Wrote the value");
		}
    }
}
