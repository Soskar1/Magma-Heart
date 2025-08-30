using System;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public interface ITilePosition
    {
        public Transform Transform { get; }
        public Vector3Int CurrentTilePosition { get; }
        public EventHandler<OnMovedEventArgs> OnMoved { get; set; }
    }
}