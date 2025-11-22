using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestTankProject.Runtime.Gameplay
{
    public readonly struct CardDataForView
    {
        public readonly Vector2Int Address;
        public readonly Sprite Icon;

        public CardDataForView(Vector2Int address, Sprite icon)
        {
            Address = address;
            Icon = icon;
        }
    }
}
