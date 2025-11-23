using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace TestTankProject.Runtime.Core.SaveLoad
{
    public class Vector2IntConverter : JsonConverter<Vector2Int>
    {
        public override void WriteJson(JsonWriter writer, Vector2Int value, JsonSerializer serializer)
        {
            if (writer.WriteState == WriteState.Object)
            {
                WriteNestedBody(writer, value);
            }
            else
            {
                writer.WriteStartObject();
                WriteNestedBody(writer, value);
                writer.WriteEndObject();
            }
        }

        public override Vector2Int ReadJson(JsonReader reader, Type objectType, Vector2Int existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            
            return new Vector2Int((int)jObject["x"], (int)jObject["y"]);
        }

        private void WriteNestedBody(JsonWriter writer, Vector2Int value)
        {
            writer.WritePropertyName("x");
            writer.WriteValue(value.x);
            writer.WritePropertyName("y");
            writer.WriteValue(value.y);
        }
    }
}
