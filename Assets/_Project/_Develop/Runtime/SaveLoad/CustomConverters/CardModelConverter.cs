using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestTankProject.Runtime.Gameplay;
using UnityEngine;

namespace TestTankProject.Runtime.Core.SaveLoad
{
    public class CardMovelConverter : JsonConverter<CardModel>
    {
        public override void WriteJson(JsonWriter writer, CardModel value, JsonSerializer serializer)
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

        public override CardModel ReadJson(JsonReader reader, Type objectType, CardModel existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            return new CardModel(new(), new(), null);
            //return new CardModel((int)jObject["x"], (int)jObject["y"]);
        }

        private void WriteNestedBody(JsonWriter writer, CardModel value)
        {
            /*writer.WritePropertyName("x");
            writer.WriteValue(value.x);
            writer.WritePropertyName("y");
            writer.WriteValue(value.y);*/
        }
    }
}
