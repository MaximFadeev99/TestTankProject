using MessagePipe;
using TestTankProject.Runtime.UI.EndGamePanel;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace TestTankProject.Runtime.UI
{
    [RequireComponent(typeof(Button))]
    public class ReturnButtonView : MonoBehaviour
    {
        private Button _button;
        private IPublisher<ReturnButtonPressedEvent> _returnPressedPublisher;
        
        [Inject]
        private void Initialize(IPublisher<ReturnButtonPressedEvent> returnPressedPublisher)
        {
            _returnPressedPublisher = returnPressedPublisher;
            
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            _returnPressedPublisher.Publish(new ReturnButtonPressedEvent());
        }

        private void Destroy()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }
    }
}