using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestTankProject.Runtime.UI.MainMenu
{
    [RequireComponent(typeof(Button))]
    internal class MainMenuButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _captionField;
        
        private Button  _button;

        internal MainMenuButtonTypes Type { get; private set; } = MainMenuButtonTypes.Undefined;

        internal event Action<MainMenuButtonTypes> Pressed;

        internal void Initialize(MainMenuButtonTypes type, string caption)
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClick);
            Type = type;
            _captionField.text = caption;
        }

        internal void Destroy()
        {
            if (_button != null)
                _button.onClick.AddListener(OnButtonClick);
            
            Destroy(gameObject);
        }

        private void OnButtonClick()
        {
            Pressed?.Invoke(Type);
        }
    }
}
