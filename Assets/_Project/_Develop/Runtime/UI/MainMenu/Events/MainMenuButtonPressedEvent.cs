using TestTankProject.Runtime.Bootstrap;

namespace TestTankProject.Runtime.UI.MainMenu
{
    [Message]
    public readonly struct MainMenuButtonPressedEvent
    {
        public readonly MainMenuButtonTypes PressedButtonType;

        public MainMenuButtonPressedEvent(MainMenuButtonTypes pressedButtonType)
        {
            PressedButtonType = pressedButtonType;
        }
    }
}