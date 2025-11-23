using System.Collections.Generic;
using TestTankProject.Runtime.Bootstrap;

namespace TestTankProject.Runtime.MainMenu
{
    [Message]
    public readonly struct SetUpMainMenuView
    {
        public readonly IReadOnlyList<MainMenuButtonData> MainMenuButtons;

        public SetUpMainMenuView(IReadOnlyList<MainMenuButtonData> mainMenuButtons)
        {
            MainMenuButtons = mainMenuButtons;
        }
    }
}
