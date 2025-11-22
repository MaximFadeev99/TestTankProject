using System;
using System.Collections.Generic;
using TestTankProject.Runtime.UI.MainMenu;

namespace TestTankProject.Runtime.MainMenu
{
    public readonly struct SetUpMainMenuView
    {
        public readonly IReadOnlyList<MainMenuButtonData> MainMenuButtons;

        public SetUpMainMenuView(IReadOnlyList<MainMenuButtonData> mainMenuButtons)
        {
            MainMenuButtons = mainMenuButtons;
        }
    }
}
