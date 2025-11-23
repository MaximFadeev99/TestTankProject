using System.IO;
using Newtonsoft.Json;
using TestTankProject.Runtime.Gameplay;
using UnityEngine;

namespace TestTankProject.Runtime.SaveLoad
{
    public class LocalGameSaver : IGameSaver
    {
        private readonly JsonSerializer _jsonSerializer;

        public LocalGameSaver(JsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        public void SaveGame(GameModel game)
        {
            string jString = string.Empty;
            
            using (var stringWriter = new StringWriter())
            {
                _jsonSerializer.Serialize(stringWriter, game);
                jString = stringWriter.ToString();
            }
            
            Debug.Log(jString);
            PlayerPrefs.SetString(RuntimeConstants.SavedGameKey, jString);    
        }

        public void DeleteSavedGame()
        {
            PlayerPrefs.DeleteKey(RuntimeConstants.SavedGameKey);  
        }
    }
}