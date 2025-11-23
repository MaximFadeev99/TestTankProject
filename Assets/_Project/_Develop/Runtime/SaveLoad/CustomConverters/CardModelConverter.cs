using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestTankProject.Runtime.Gameplay;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using SerializationConstants = TestTankProject.Runtime.RuntimeConstants.SerializationConstants;

namespace TestTankProject.Runtime.Core.SaveLoad
{
    public class CardModelConverter : JsonConverter<CardModel>
    {
        public override void WriteJson(JsonWriter writer, CardModel value, JsonSerializer serializer)
        {
            if (writer.WriteState == WriteState.Object)
            {
                WriteNestedBody(writer, value, serializer);
            }
            else
            {
                writer.WriteStartObject();
                WriteNestedBody(writer, value, serializer);
                writer.WriteEndObject();
            }
        }

        public override CardModel ReadJson(JsonReader reader, Type objectType, CardModel existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            
            Vector2Int address = serializer.Deserialize<Vector2Int>
                (jObject[SerializationConstants.Address].CreateReader());
            Vector2Int matchingCardAddress = serializer.Deserialize<Vector2Int>
                (jObject[SerializationConstants.MatchingCardAddress].CreateReader());
            AssetReferenceSprite iconReference = new(jObject[SerializationConstants.IconReference].ToString());
            CardStatus status = jObject[SerializationConstants.Status].ToObject<CardStatus>();
            
            return new(address, matchingCardAddress, iconReference, status);
        }

        private void WriteNestedBody(JsonWriter writer, CardModel value, JsonSerializer serializer)
        {
            writer.WritePropertyName(SerializationConstants.Address);
            serializer.Serialize(writer, value.Address);
            writer.WritePropertyName(SerializationConstants.MatchingCardAddress);
            serializer.Serialize(writer, value.MatchingCardAddress);
            writer.WritePropertyName(SerializationConstants.IconReference);
            writer.WriteValue(value.IconReference.AssetGUID);
            writer.WritePropertyName(SerializationConstants.Status);
            writer.WriteValue(value.Status);
        }
    }
}