using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem
{
    public class CombatTile
    {
        public Vector3Int Position { get; private set; }

        public CombatTile(Vector3Int position) => Position = position;
    }
}
