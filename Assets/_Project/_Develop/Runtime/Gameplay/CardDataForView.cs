using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestTankProject.Runtime.Gameplay
{
    public readonly struct CardDataForView
    {
        public readonly Vector2Int Address;
        public readonly Sprite Icon;
        public readonly bool IsVisible;

        public CardDataForView(Vector2Int address, Sprite icon, bool isVisible)
        {
            Address = address;
            Icon = icon;
            IsVisible = isVisible;
        }
    }
}
