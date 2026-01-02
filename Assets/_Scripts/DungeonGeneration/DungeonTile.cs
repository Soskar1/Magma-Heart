using UnityEngine;

namespace MagmaHeart.DungeonGeneration
{
    public enum TileType
    {
        Floor,
        Wall
    }

    public class DungeonTile
    {
        private readonly Vector2Int m_position;
        private TileType m_type;

        public Vector2Int Position => m_position;
        public TileType Type { get { return m_type; } set { m_type = value; } }

        public DungeonTile(Vector2Int position, TileType type)
        {
            m_position = position;
            m_type = type;
        }
    }
}