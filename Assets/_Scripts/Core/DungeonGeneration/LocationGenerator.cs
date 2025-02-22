using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.Dungeon
{
    public class LocationGenerator : MonoBehaviour
    {
        private LocationRenderer m_renderer;

        [SerializeField] private Tilemap m_tilemap;
        [SerializeField] private TileBase m_floorTile;
        [SerializeField] private int m_randomWalkIterations;
        [SerializeField] private int m_xBorderSize;
        [SerializeField] private int m_yBorderSize;

        [SerializeField] private int m_xSize;
        [SerializeField] private int m_ySize;

        [SerializeField] private int m_tilesToPlace;

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
            IRoomGenerator generator1 = new BoxedRoomGenerator(roomData, m_xSize, m_ySize); 
            IRoomGenerator generator2 = new DiffusionLimitedAggregatoinRoomGenerator(roomData, m_tilesToPlace);
            IRoomGenerator generator3 = new RandomWalkRoomGenerator(roomData, m_randomWalkIterations);
            IRoomGenerator generator4 = new DiffusionLimitedAggregatoinRoomGenerator(roomData, m_tilesToPlace);
            IRoomGenerator generator5 = new RandomWalkRoomGenerator(roomData, m_randomWalkIterations);
            HashSet<Vector2Int> generatedTiles = generator5.GenerateRoom(
                generator4.GenerateRoom(
                generator3.GenerateRoom(
                generator2.GenerateRoom(
                generator1.GenerateRoom(null)))));
            m_renderer.DrawTiles(generatedTiles);

            // Connect rooms with corridors

            // Post processing
        }

        public void ClearLocation() => m_renderer.Clear();
    }
}

