using System;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class OnMovedEventArgs : EventArgs
    {
        public Vector3Int From { get; init; }
        public Vector3Int To { get; init; }

        public OnMovedEventArgs(Vector3Int from, Vector3Int to)
        {
            From = from;
            To = to;
        }
    }
}