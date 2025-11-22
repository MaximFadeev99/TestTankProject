namespace TestTankProject.Runtime.UI.MainMenu
{
    public readonly struct MainMenuButtonPressedEvent
    {
        public readonly MainMenuButtonTypes PressedButtonType;

        public MainMenuButtonPressedEvent(MainMenuButtonTypes pressedButtonType)
        {
            PressedButtonType = pressedButtonType;
        }
    }
}