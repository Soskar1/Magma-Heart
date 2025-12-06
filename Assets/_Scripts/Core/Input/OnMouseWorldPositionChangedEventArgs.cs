using System;
using UnityEngine;

namespace MagmaHeart.Core.Input
{
    public class OnMouseWorldPositionChangedEventArgs : EventArgs
    {
        public Vector2 WorldPosition { get; init; }
        public OnMouseWorldPositionChangedEventArgs(Vector2 worldPosition) => WorldPosition = worldPosition;
    }
}
