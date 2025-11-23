using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestTankProject.Runtime.Gameplay;
using UnityEngine;

namespace TestTankProject.Runtime.SaveLoad
{
    public class LocalGameLoader : IGameLoader
    {
        private readonly JsonSerializer _jsonSerializer;

        public LocalGameLoader(JsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }


        public GameModel LoadSavedGame()
        {
            string jString = PlayerPrefs.GetString(RuntimeConstants.SavedGameKey, string.Empty);
            
            if (jString == string.Empty)
                return null;
            
            JObject jObject = JObject.Parse(jString);
            GameModel gameModel = _jsonSerializer.Deserialize<GameModel>(jObject.CreateReader());
            return gameModel;
        }
    }
}