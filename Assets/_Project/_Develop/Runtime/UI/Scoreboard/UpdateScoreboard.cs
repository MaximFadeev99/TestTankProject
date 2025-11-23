using TestTankProject.Runtime.Bootstrap;

namespace TestTankProject.Runtime.UI.Scoreboard
{
    [Message]
    public readonly struct UpdateScoreboard
    {
        public readonly int CurrentPoints;
        public readonly int BasePoints;
        public readonly int BonusPoints;
        public readonly int CurrentMatches;
        public readonly int TotalMatchAttempts;

        public UpdateScoreboard(int currentPoints, int basePoints, int bonusPoints, int currentMatches, int totalMatchAttempts)
        {
            CurrentPoints = currentPoints;
            BasePoints = basePoints;
            BonusPoints = bonusPoints;
            CurrentMatches = currentMatches;
            TotalMatchAttempts = totalMatchAttempts;
        }
    }
}