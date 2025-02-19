using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.Dungeon
{
    public class LocationGenerator : MonoBehaviour
    {
        private RandomWalkRoomGenerator m_roomGenerator;
        private LocationRenderer m_renderer;

        [SerializeField] private Tilemap m_tilemap;
        [SerializeField] private TileBase m_floorTile;
        [SerializeField] private int m_walkLength;

        private void Awake()
        {
            m_roomGenerator = new RandomWalkRoomGenerator(m_walkLength);
            m_renderer = new LocationRenderer(m_tilemap, m_floorTile);
        }

        public void Start()
        {
            GenerateRoom(Vector2Int.zero);
        }

        public void GenerateRoom(in Vector2Int position)
        {
            HashSet<Vector2Int> path = m_roomGenerator.GenerateRoom(position);
            m_renderer.DrawTiles(path);
        }
    }
}

