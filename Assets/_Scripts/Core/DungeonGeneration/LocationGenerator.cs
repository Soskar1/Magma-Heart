using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private int m_randomWalkIterations;

        private void Awake()
        {
            m_roomGenerator = new RandomWalkRoomGenerator(m_walkLength);
            m_renderer = new LocationRenderer(m_tilemap, m_floorTile);
        }

        public void Start()
        {
            GenerateLocation(Vector2Int.zero);
        }

        public void GenerateLocation() => GenerateLocation(Vector2Int.zero);

        public void GenerateLocation(in Vector2Int position)
        {
            // Generate spaces for rooms

            // For each space generate a room
            GenerateRoom(position);

            // Connect rooms with corridors

            // Post processing
        }

        private void GenerateRoom(in Vector2Int position)
        {
            HashSet<Vector2Int> roomFloorPositions = new HashSet<Vector2Int>();
            Vector2Int randomWalkStartPosition = position;

            for (int i = 0; i < m_randomWalkIterations; ++i)
            {
                HashSet<Vector2Int> randomWalkPath = m_roomGenerator.GenerateRoom(randomWalkStartPosition);
                roomFloorPositions.UnionWith(randomWalkPath);
                randomWalkStartPosition = roomFloorPositions.ElementAt(Random.Range(0 , roomFloorPositions.Count));
            }

            m_renderer.DrawTiles(roomFloorPositions);
        }

        public void ClearLocation() => m_renderer.Clear();
    }
}

