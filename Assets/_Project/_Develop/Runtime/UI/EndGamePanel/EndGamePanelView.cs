using System;
using MessagePipe;
using TestTankProject.Runtime.UI.EndGamePanel.Commands;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace TestTankProject.Runtime.UI.EndGamePanel
{
    public class EndGamePanelView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _captionField;
        [SerializeField] private Button _returnButton;
        
        private GameObject _gameObject;
        
        private IPublisher<ReturnButtonPressedEvent> _returnPressedPublisher;
        private IDisposable _disposableForSubscriptions;
        
        [Inject]
        private void Initialize(ISubscriber<DrawEndGamePanel> drawCommandSubscriber, 
            IPublisher<ReturnButtonPressedEvent> returnPressedPublisher,
            ISubscriber<ReturnButtonPressedEvent> returnPressedSubscriber)
        {
            _gameObject = gameObject;
            _returnPressedPublisher = returnPressedPublisher;
            _returnButton.onClick.AddListener(OnReturnButtonClick);
            
            DisposableBagBuilder bagBuilder = DisposableBag.CreateBuilder();
            drawCommandSubscriber.Subscribe(OnDrawCommand).AddTo(bagBuilder);
            returnPressedSubscriber.Subscribe(OnReturnPressedEvent).AddTo(bagBuilder);
            _disposableForSubscriptions = bagBuilder.Build();
        }

        private void OnDrawCommand(DrawEndGamePanel drawCommand)
        {
            _captionField.text = drawCommand.Caption;
            
            if (_gameObject == null)
                return;
            
            _gameObject.SetActive(true);
        }

        private void OnReturnButtonClick()
        {
            _returnPressedPublisher.Publish(new ReturnButtonPressedEvent());
            _gameObject.SetActive(false);
        }

        private void OnReturnPressedEvent(ReturnButtonPressedEvent _)
        {
            Destroy(_gameObject);
            OnDestroy();
        }

        private void OnDestroy()
        {
            _disposableForSubscriptions?.Dispose();
            _returnButton.onClick.RemoveListener(OnReturnButtonClick);
        }
    }
}
