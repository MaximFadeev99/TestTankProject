using TestTankProject.Runtime.Bootstrap;
using UnityEngine;

namespace TestTankProject.Runtime.UserInput
{
    [Message]
    public readonly struct UserClickRegisteredEvent
    {
        public readonly Vector2 ClickScreenPosition;

        public UserClickRegisteredEvent(Vector2 clickScreenPosition)
        {
            ClickScreenPosition = clickScreenPosition;
        }
    }
}
