using System;
using MessagePipe;
using TestTankProject.UserInput;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace TestTankProject.Runtime.UserInput
{
    public class InputLogger : IDisposable
    {
        private readonly InputMap _inputMap;
        private readonly IPublisher<UserClickRegisteredEvent> _userClickPublisher;
        
        private Vector2 _interactionScreenPosition;

        public InputLogger(IPublisher<UserClickRegisteredEvent> userClickPublisher)
        {
            _userClickPublisher = userClickPublisher;
            _inputMap = new();
            _inputMap.Enable();
            _inputMap.Default.Click.performed += OnClickPerformed;
            _inputMap.Default.ClickPosition.performed += OnCursorMoved;
        }

        private void OnClickPerformed(CallbackContext context)
        {
            _userClickPublisher.Publish(new(_interactionScreenPosition));
        }
        
        private void OnCursorMoved(CallbackContext context)
        {
            _interactionScreenPosition = context.ReadValue<Vector2>();
        } 
        
        public void Dispose()
        {
            _inputMap.Default.Click.performed -= OnClickPerformed;
            _inputMap.Default.ClickPosition.performed -= OnCursorMoved;
            
            _inputMap.Disable();
            _inputMap.Dispose();
        }
    }
}
