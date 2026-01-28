using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Archipelago.MultiClient.Net.Converters
{
	internal class IntEnumConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			var enumType = Nullable.GetUnderlyingType(objectType) ?? objectType;
			if (!objectType.IsEnum)
			{
				return false;
			}

			var enumAssembly = enumType.Assembly;
			var converterAssembly = typeof(IntEnumConverter).Assembly;
			if (enumAssembly != converterAssembly)
			{
				return false;
			}

			var message = $"{Environment.NewLine}Can convert an object of type: `{objectType}` ({enumAssembly}) using the {nameof(IntEnumConverter)} ({converterAssembly})";
			File.AppendAllText("multiclientlog.txt", message);

			return true;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}

			writer.WriteValue(Convert.ToInt32(value));
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var readerValue = reader.Value;
			var message = $"{Environment.NewLine}Trying to Read Json. Object Type: `{objectType}`. Value: {readerValue}";
			File.AppendAllText("multiclientlog.txt", message);
			var enumType = Nullable.GetUnderlyingType(objectType) ?? objectType;
			return Enum.ToObject(enumType, Convert.ToInt32(readerValue));
		}
	}
}
