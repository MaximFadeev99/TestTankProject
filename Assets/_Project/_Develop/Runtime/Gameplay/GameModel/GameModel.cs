using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TestTankProject.Runtime.Core.SaveLoad;
using UnityEngine;

namespace TestTankProject.Runtime.Gameplay
{
    [JsonConverter(typeof(GameModelConverter))]
    public class GameModel
    {
        internal readonly float InitialCardShowTime;
        internal readonly float CardDisappearDelay;
        internal readonly IReadOnlyList<CardModel> Cards;
        internal readonly int RequiredMatches;
        internal readonly CardModel[] SelectedCards;
        internal readonly int PointsPerMatch;
        internal readonly int PointsPerMatchStreak;

        internal GameStatus Status;

        internal int CurrentPoints { get; private set; }
        internal int CurrentMatchStreak { get; private set; }
        internal int BonusPoints => CurrentMatchStreak * PointsPerMatchStreak;
        internal int CurrentMatches { get; private set; }
        internal int TotalMatchAttempts { get; private set; }

        internal event Action<Vector2Int> ShowCard;
        internal event Action<Vector2Int, Vector2Int> CardsMatched;
        internal event Action<Vector2Int, Vector2Int> CardsMismatched;
        internal event Action TurnCompleted;
        internal event Action GameCompleted;
        
        public GameModel(float initialCardShowTime, float cardDisappearDelay, int pointsPerMatch, 
            int pointsPerMatchStreak, IReadOnlyList<CardModel> cards, int currentPoints, int currentMatchStreak,
            int currentMatches, int totalMatchAttempts)
        {
            InitialCardShowTime = initialCardShowTime;
            CardDisappearDelay = cardDisappearDelay;
            PointsPerMatch = pointsPerMatch;
            PointsPerMatchStreak = pointsPerMatchStreak;
            Cards = cards;
            CurrentPoints = currentPoints;
            CurrentMatchStreak = currentMatchStreak;
            CurrentMatches = currentMatches;
            TotalMatchAttempts = totalMatchAttempts;
            
            RequiredMatches = cards.Count / RuntimeConstants.MatchingCardCount;
            SelectedCards = new CardModel[RuntimeConstants.MatchingCardCount];
            Status = GameStatus.BeingInitialized;
        }

        public void SelectCard(Vector2Int cardAddress)
        {
            CardModel targetCard = Cards.First(card => card.Address == cardAddress);
            
            if (targetCard.Status != CardStatus.Unmatched)
                return;
            
            if (SelectedCards[0] == null)
            {
                SelectedCards[0] = targetCard;
                ShowCard?.Invoke(targetCard.Address);
                return;
            }
            
            SelectedCards[1] = targetCard;
            ShowCard?.Invoke(targetCard.Address);
            TotalMatchAttempts++;

            bool isMatch = SelectedCards[0].Address == SelectedCards[1].MatchingCardAddress &&
                           SelectedCards[1].Address == SelectedCards[0].MatchingCardAddress;

            if (isMatch)
            {
                CurrentMatches++;
                CurrentPoints += PointsPerMatch + BonusPoints;
                CurrentMatchStreak++;
                SelectedCards[0].Status = CardStatus.Matched;
                SelectedCards[1].Status = CardStatus.Matched;
                CardsMatched?.Invoke(SelectedCards[0].Address, SelectedCards[1].Address);
            }
            else
            {
                CurrentMatchStreak = 0;
                CardsMismatched?.Invoke(SelectedCards[0].Address, SelectedCards[1].Address);
            }

            SelectedCards[0] = null;
            SelectedCards[1] = null;
            TurnCompleted?.Invoke();
            
            if (CurrentMatches != RequiredMatches)
                return;

            Status = GameStatus.Completed;
            GameCompleted?.Invoke();
        }
    }

    public enum GameStatus
    {
        Undefined = 0,
        BeingInitialized,
        Running,
        Completed
    }
}
