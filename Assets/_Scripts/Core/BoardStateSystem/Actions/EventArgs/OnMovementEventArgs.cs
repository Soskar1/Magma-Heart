using System;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public class OnMovementEventArgs : EventArgs
    {
        public Vector3Int From { get; init; }
        public Vector3Int To { get; init; }

        public OnMovementEventArgs(Vector3Int from, Vector3Int to)
        {
            From = from;
            To = to;
        }
    }
}