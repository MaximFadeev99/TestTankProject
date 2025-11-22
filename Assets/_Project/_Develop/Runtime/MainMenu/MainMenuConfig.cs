using System;
using System.Collections.Generic;
using TestTankProject.Runtime.UI.MainMenu;
using UnityEngine;
using UnityEngine.Serialization;

namespace TestTankProject.Runtime.MainMenu
{
    [CreateAssetMenu(fileName = "MainMenuConfig_New", menuName = "_Project/MainMenuConfig")]
    public class MainMenuConfig : ScriptableObject
    {
        [SerializeField] private List<MainMenuButtonData> _mainMenuButtons;
        
        public IReadOnlyList<MainMenuButtonData> MainMenuButtons => _mainMenuButtons;
    }
    
    [Serializable]
    public struct MainMenuButtonData
    {
        [FormerlySerializedAs("BType")] [FormerlySerializedAs("ButtonType")] public MainMenuButtonTypes Type;
        public string Caption;
    }
}
