using System;
using UnityEngine;

namespace MagmaHeart.Core.Input
{
    public class OnMousePositionChangedEventArgs : EventArgs
    {
        public Vector2 Position { get; init; }
        public OnMousePositionChangedEventArgs(Vector2 position) => Position = position;
    }
}
