using UnityEngine;

namespace TestTankProject.Runtime
{
    public static class RuntimeConstants
    {
        public const int MatchingCardCount = 2;
        public const string SavedGameKey = "SavedGame";
        public static int CardsLayerMask = LayerMask.GetMask("Cards");

        public static class SerializationConstants
        {
            public const string Address = "Address";
            public const string MatchingCardAddress = "MatchingCardAddress";
            public const string IconReference  = "IconReference";
            public const string Status = "Status";
            
            public const string InitialCardShowTime = "InitialCardShowTime";
            public const string CardDisappearDelay = "CardDisappearDelay";
            public const string Cards = "Cards";
            public const string RequiredMatches = "RequiredMatches";
            public const string SelectedCards = "SelectedCards";
            public const string PointsPerMatch = "PointsPerMatch";
            public const string PointsPerMatchStreak = "PointsPerMatchStreak";
            public const string CurrentPoints = "CurrentPoints";
            public const string CurrentMatchStreak = "CurrentMatchStreak";
            public const string CurrentMatches = "CurrentMatches";
            public const string TotalMatchAttempts = "TotalMatchAttempts";
        }
    }
}
