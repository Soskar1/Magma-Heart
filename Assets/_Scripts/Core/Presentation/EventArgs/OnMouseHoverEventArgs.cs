using System;
using UnityEngine;

namespace MagmaHeart.Core.Presentation
{
    public class OnMouseHoverEventArgs : EventArgs
    {
        public Vector2 WorldPosition { get; init; }

        public OnMouseHoverEventArgs(Vector2 worldPosition) => WorldPosition = worldPosition;
    }
}