using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.Dungeon
{
    public class GameGrid : MonoBehaviour
    {
        [SerializeField] private Tilemap m_floor;
        [SerializeField] private Tilemap m_walls;
        [SerializeField] private Tilemap m_corridors;

        [SerializeField] private TileBase m_floorTile;
        [SerializeField] private TileBase m_wallTile;

        public Tilemap Floor => m_floor;
        public Tilemap Walls => m_walls;
        public Tilemap Corridors => m_corridors;

        public TileBase FloorTile => m_floorTile;
        public TileBase WallTile => m_wallTile;
    }
}