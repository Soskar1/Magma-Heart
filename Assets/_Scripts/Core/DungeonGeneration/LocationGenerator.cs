using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.Dungeon
{
    public class LocationGenerator : MonoBehaviour
    {
        private LocationRenderer m_renderer;
        private IRoomGenerator m_roomGenerator;

        [SerializeField] private Tilemap m_tilemap;
        [SerializeField] private TileBase m_floorTile;
        [SerializeField] private int m_walkLength;
        [SerializeField] private int m_randomWalkIterations;
        [SerializeField] private int m_xBorderSize;
        [SerializeField] private int m_yBorderSize;

        [SerializeField] private int m_xSize;
        [SerializeField] private int m_ySize;

        private void Awake()
        {
            m_renderer = new LocationRenderer(m_tilemap, m_floorTile);
        }

        public void GenerateLocation() => GenerateLocation(Vector2Int.zero);

        public void GenerateLocation(in Vector2Int position)
        {
            // Generate spaces for rooms

            // For each space generate a room
            RoomData roomData = new RoomData(position, m_xBorderSize, m_yBorderSize);
            m_roomGenerator = new BoxedRoomGenerator(roomData, m_xSize, m_ySize);
            GenerateRoom(position);

            // Connect rooms with corridors

            // Post processing
        }

        private void GenerateRoom(in Vector2Int position)
        {
            HashSet<Vector2Int> roomFloor = m_roomGenerator.GenerateRoom(position);
            m_renderer.DrawTiles(roomFloor);
        }

        public void ClearLocation() => m_renderer.Clear();
    }
}

