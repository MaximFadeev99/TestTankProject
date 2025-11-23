using TestTankProject.Runtime.Gameplay;

namespace TestTankProject.Runtime.SaveLoad
{
    public interface IGameSaver
    {
        public void SaveGame(GameModel game);
        public void DeleteSavedGame();
    }
}