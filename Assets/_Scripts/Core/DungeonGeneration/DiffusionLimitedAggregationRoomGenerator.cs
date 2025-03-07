using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class DiffusionLimitedAggregatoinRoomGenerator : IRoomGenerator
    {
        private readonly RoomData m_roomData;
        private readonly RandomWalk m_randomWalk;
        private readonly int m_tilesToPlace;
        private const int START_POINT_OFFSET = 12;

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
        }

        public HashSet<Vector2Int> GenerateRoom(in HashSet<Vector2Int> generatedTiles)
        {
            HashSet<Vector2Int> tiles = new HashSet<Vector2Int>() { m_roomData.WorldPosition };
            int tilesToPlace = m_tilesToPlace;

            if (generatedTiles != null)
                tiles = generatedTiles;

            int localLeftBorder = generatedTiles.Min(vec => vec.x) - START_POINT_OFFSET;
            int localRightBorder = generatedTiles.Max(vec => vec.x) + START_POINT_OFFSET;
            int localBottomBorder = generatedTiles.Min(vec => vec.y) - START_POINT_OFFSET;
            int localTopBorder = generatedTiles.Max(vec => vec.y) + START_POINT_OFFSET;

            localLeftBorder = Mathf.Max(localLeftBorder, m_roomData.LeftBorder);
            localRightBorder = Mathf.Min(localRightBorder, m_roomData.RightBorder);
            localBottomBorder = Mathf.Max(localBottomBorder, m_roomData.BottomBorder);
            localTopBorder = Mathf.Min(localTopBorder, m_roomData.TopBorder);

            while (tilesToPlace > 0)
            {
                List<Vector2Int> startPoints = new List<Vector2Int>() {
                    new Vector2Int(localLeftBorder / 2, localBottomBorder),
                    new Vector2Int(localLeftBorder, localBottomBorder),
                    new Vector2Int(localLeftBorder, localTopBorder / 2),
                    new Vector2Int(localLeftBorder, localTopBorder),
                    new Vector2Int(localRightBorder / 2, localTopBorder),
                    new Vector2Int(localRightBorder, localTopBorder),
                    new Vector2Int(localRightBorder, localTopBorder / 2),
                    new Vector2Int(localRightBorder, localBottomBorder),
                };

                Vector2Int randomWalkStartPoint = startPoints[Random.Range(0, startPoints.Count)];
                Vector2Int currentPosition = randomWalkStartPoint;
                bool hitTile = false;

                while (!hitTile)
                {
                    Vector2Int newPosition = currentPosition + m_randomWalk.TakeRandomDirection();

                    if (newPosition.x > localRightBorder)
                        newPosition.x = localLeftBorder;

                    if (newPosition.x < localLeftBorder)
                        newPosition.x = localRightBorder;

                    if (newPosition.y > localTopBorder)
                        newPosition.y = localBottomBorder;

                    if (newPosition.y < localBottomBorder)
                        newPosition.y = localTopBorder;

                    if (!tiles.Contains(currentPosition) && tiles.Contains(newPosition))
                        hitTile = true;
                    else
                        currentPosition = newPosition;
                }

                tiles.Add(currentPosition);
                --tilesToPlace;

                if (currentPosition.x - START_POINT_OFFSET < localLeftBorder)
                    localLeftBorder = Mathf.Max(currentPosition.x - START_POINT_OFFSET, m_roomData.LeftBorder);
                else if (currentPosition.x + START_POINT_OFFSET > localRightBorder)
                    localRightBorder = Mathf.Min(currentPosition.x + START_POINT_OFFSET, m_roomData.RightBorder);
                
                if (currentPosition.y - START_POINT_OFFSET < localBottomBorder)
                    localBottomBorder = Mathf.Max(currentPosition.y - START_POINT_OFFSET, m_roomData.BottomBorder);
                else if (currentPosition.y + START_POINT_OFFSET > localTopBorder)
                    localTopBorder = Mathf.Min(currentPosition.y + START_POINT_OFFSET, m_roomData.TopBorder);
            }

            return tiles;
        }
    }
}