using System;
using UnityEngine;

namespace MagmaHeart.Core.Input
{
    public class OnMovementKeyPressedEventArgs : EventArgs
    {
        public Vector2 Movement { get; init; }
        public OnMovementKeyPressedEventArgs(Vector2 movement)
        {
            Movement = movement;
        }
    }
}
