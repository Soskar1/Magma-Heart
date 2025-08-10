using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public interface ITilePosition
    {
        public Transform Transform { get; }
        public Vector3Int CurrentTilePosition { get; set; }
    }
}