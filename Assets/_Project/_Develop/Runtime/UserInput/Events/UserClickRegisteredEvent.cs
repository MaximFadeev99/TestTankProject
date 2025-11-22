using UnityEngine;

namespace TestTankProject.Runtime.UserInput
{
    public readonly struct UserClickRegisteredEvent
    {
        public readonly Vector2 ClickScreenPosition;

        public UserClickRegisteredEvent(Vector2 clickScreenPosition)
        {
            ClickScreenPosition = clickScreenPosition;
        }
    }
}
