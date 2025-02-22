using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class DiffusionLimitedAggregatoinRoomGenerator : IRoomGenerator
    {
        private readonly RoomData m_roomData;
        private readonly RandomWalk m_randomWalk;
        private readonly List<Vector2Int> m_startPoints;
        private readonly int m_tilesToPlace;

        public DiffusionLimitedAggregatoinRoomGenerator(RoomData roomData, int tilesToPlace)
        {
            m_roomData = roomData;
            m_tilesToPlace = tilesToPlace;
            m_randomWalk = new RandomWalk(new List<Vector2Int>() {
                Vector2Int.left,
                Vector2Int.right,
                Vector2Int.up,
                Vector2Int.down,
                new Vector2Int(1, 1),
                new Vector2Int(1, -1),
                new Vector2Int(-1, -1),
                new Vector2Int(-1, 1)
            });

            m_startPoints = new List<Vector2Int>() {
                new Vector2Int(m_roomData.LeftBorder, m_roomData.BottomBorder),
                new Vector2Int(m_roomData.LeftBorder, m_roomData.UpperBorder),
                new Vector2Int(m_roomData.RightBorder, m_roomData.UpperBorder),
                new Vector2Int(m_roomData.RightBorder, m_roomData.BottomBorder)
            };
        }

        public HashSet<Vector2Int> GenerateRoom(in HashSet<Vector2Int> generatedTiles)
        {
            HashSet<Vector2Int> tiles = new HashSet<Vector2Int>() { m_roomData.WorldPosition };
            int tilesToPlace = m_tilesToPlace;

            if (generatedTiles != null)
                tiles = generatedTiles;

            while (tilesToPlace > 0)
            {
                Vector2Int randomWalkStartPoint = m_startPoints[Random.Range(0, m_startPoints.Count)];
                Vector2Int currentPosition = randomWalkStartPoint;
                bool hitTile = false;

                while (!hitTile)
                {
                    Vector2Int newPosition = currentPosition + m_randomWalk.TakeRandomDirection();

                    if (newPosition.x > m_roomData.RightBorder)
                        newPosition.x = m_roomData.LeftBorder;

                    if (newPosition.x < m_roomData.LeftBorder)
                        newPosition.x = m_roomData.RightBorder;

                    if (newPosition.y > m_roomData.UpperBorder)
                        newPosition.y = m_roomData.BottomBorder;

                    if (newPosition.y < m_roomData.BottomBorder)
                        newPosition.y = m_roomData.UpperBorder;

                    if (!tiles.Contains(currentPosition) && tiles.Contains(newPosition))
                        hitTile = true;
                    else
                        currentPosition = newPosition;
                }

                tiles.Add(currentPosition);
                --tilesToPlace;
            }

            return tiles;
        }
    }
}