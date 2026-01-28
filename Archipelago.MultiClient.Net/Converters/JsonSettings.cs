using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Archipelago.MultiClient.Net.Converters
{
    public static class JsonSettings
    {
        static JsonSerializerSettings _settings = null;

        /// <summary>
        /// Gets the default serializer settings to be used in Multiclient.net
        /// </summary>
        /// <returns></returns>
        public static JsonSerializerSettings GetSerializerSettings()
        {
            if (_settings == null)
            {
                _settings = new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter>(), // Cleared list of converters, in case another assembly is adding some to the defaults
                };
            }

            return _settings;
        }
    }
}
