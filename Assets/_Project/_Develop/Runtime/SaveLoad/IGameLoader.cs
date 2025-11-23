using TestTankProject.Runtime.Gameplay;

namespace TestTankProject.Runtime.SaveLoad
{
    public interface IGameLoader
    {
        public GameModel LoadSavedGame();
    }
}