using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestTankProject.Runtime.Gameplay;
using SerializationConstants = TestTankProject.Runtime.RuntimeConstants.SerializationConstants;

namespace TestTankProject.Runtime.Core.SaveLoad
{
    public class GameModelConverter : JsonConverter<GameModel>
    {
        public override void WriteJson(JsonWriter writer, GameModel value, JsonSerializer serializer)
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

        public override GameModel ReadJson(JsonReader reader, Type objectType, GameModel existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            
            float initialCardShowTime = float.Parse(jObject[SerializationConstants.InitialCardShowTime].ToString());
            float cardDisappearDelay = float.Parse(jObject[SerializationConstants.CardDisappearDelay].ToString()); 
            List<CardModel> cards = serializer.Deserialize<List<CardModel>>
                (jObject[SerializationConstants.Cards].CreateReader());
            int pointsPerMatch = int.Parse(jObject[SerializationConstants.PointsPerMatch].ToString()); 
            int pointsPerMatchStreak = int.Parse(jObject[SerializationConstants.PointsPerMatchStreak].ToString()); 
            int currentPoints = int.Parse(jObject[SerializationConstants.CurrentPoints].ToString()); 
            int currentMatchStreak = int.Parse(jObject[SerializationConstants.CurrentMatchStreak].ToString()); 
            int currentMatches = int.Parse(jObject[SerializationConstants.CurrentMatches].ToString());
            int totalMatchAttempts = int.Parse(jObject[SerializationConstants.TotalMatchAttempts].ToString());
            
            return new(initialCardShowTime, cardDisappearDelay,pointsPerMatch, pointsPerMatchStreak, cards, 
                currentPoints, currentMatchStreak, currentMatches, totalMatchAttempts);
        }

        private void WriteNestedBody(JsonWriter writer, GameModel value, JsonSerializer serializer)
        {
            writer.WritePropertyName(SerializationConstants.InitialCardShowTime);
            writer.WriteValue(value.InitialCardShowTime);
            
            writer.WritePropertyName(SerializationConstants.CardDisappearDelay);
            writer.WriteValue(value.CardDisappearDelay);
            
            writer.WritePropertyName(SerializationConstants.Cards);
            serializer.Serialize(writer, value.Cards);
            
            writer.WritePropertyName(SerializationConstants.PointsPerMatch);
            writer.WriteValue(value.PointsPerMatch);
            
            writer.WritePropertyName(SerializationConstants.PointsPerMatchStreak);
            writer.WriteValue(value.PointsPerMatchStreak);
            
            writer.WritePropertyName(SerializationConstants.CurrentPoints);
            writer.WriteValue(value.CurrentPoints);
            
            writer.WritePropertyName(SerializationConstants.CurrentMatchStreak);
            writer.WriteValue(value.CurrentMatchStreak);
            
            writer.WritePropertyName(SerializationConstants.CurrentMatches);
            writer.WriteValue(value.CurrentMatches);
            
            writer.WritePropertyName(SerializationConstants.TotalMatchAttempts);
            writer.WriteValue(value.TotalMatchAttempts);
        }
    }
}