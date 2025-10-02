using System;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class OnAttackEventArgs : EventArgs
    {
        public Vector3Int EntityPosition { get; init; }

        public OnAttackEventArgs(Vector3Int entityPosition) => EntityPosition = entityPosition;
    }
}