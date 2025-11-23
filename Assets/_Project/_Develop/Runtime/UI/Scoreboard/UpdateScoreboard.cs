namespace TestTankProject.Runtime.UI.Scoreboard
{
    public readonly struct UpdateScoreboard
    {
        public readonly int CurrentPoints;
        public readonly int BonusPoints;
        public readonly int CurrentMatches;
        public readonly int TotalMatchAttempts;

        public UpdateScoreboard(int currentPoints, int bonusPoints, int currentMatches, int totalMatchAttempts)
        {
            CurrentPoints = currentPoints;
            BonusPoints = bonusPoints;
            CurrentMatches = currentMatches;
            TotalMatchAttempts = totalMatchAttempts;
        }
    }
}