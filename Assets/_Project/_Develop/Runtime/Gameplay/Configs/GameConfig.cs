using System;
using TestTankProject.Runtime.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace TestTankProject.Runtime.Gameplay
{
    [CreateAssetMenu(fileName = "GameConfig_New", menuName = "TestTankProject/GameConfig", order = 51)]
    public class GameConfig : ScriptableObject
    {
        public bool StartGameWithMe = false;
        public Vector2Int PlayingFieldSize = new Vector2Int(4,4);
        public float SpacingBetweenCards = 0.5f;
        public float InitialCardDemonstrationTime = 3f;
        public float CardDisappearDelay = 1.1f;
        public bool ShallShuffleCards = true;
        public int PointsPerMatch = 10;
        public int PointsPerMatchStreak = 5;

        private void OnValidate()
        {
            if (PlayingFieldSize.x > 10 || PlayingFieldSize.x < 1)
            {
                CustomLogger.Log(nameof(GameConfig), $"You can't have a playing field with more than 10 cards on either dimension!", 
                    MessageTypes.Error, RecipientTypes.GD);
                PlayingFieldSize.x = Mathf.Clamp(PlayingFieldSize.x, 1, 10);
            }
            
            if (PlayingFieldSize.y > 10 || PlayingFieldSize.y < 1)
            {
                CustomLogger.Log(nameof(GameConfig), $"You can't have a playing field with more than 10 cards on either dimension!", 
                    MessageTypes.Error, RecipientTypes.GD);
                PlayingFieldSize.y = Mathf.Clamp(PlayingFieldSize.y, 1, 10);
            }

            if (PlayingFieldSize.x * PlayingFieldSize.y % RuntimeConstants.MatchingCardCount != 0)
            {
                CustomLogger.Log(nameof(GameConfig), $"You can't have a playing field {PlayingFieldSize.x} by {PlayingFieldSize.y}, " +
                                                     $"because then there will be uneven card number!", 
                    MessageTypes.Error, RecipientTypes.GD);
            }
        }
    }
}
