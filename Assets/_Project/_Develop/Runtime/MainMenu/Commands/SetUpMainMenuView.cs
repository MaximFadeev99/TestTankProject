using System;
using System.Collections.Generic;
using TestTankProject.Runtime.Bootstrap;
using TestTankProject.Runtime.UI.MainMenu;

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
