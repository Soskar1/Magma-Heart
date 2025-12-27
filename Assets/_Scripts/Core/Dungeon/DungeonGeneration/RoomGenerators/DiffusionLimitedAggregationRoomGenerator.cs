using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace MagmaHeart.Core.Dungeon
{
    public class DiffusionLimitedAggregatoinRoomGenerator : IRoomGenerator
    {
        private readonly RandomWalk m_randomWalk;
        private readonly int m_tilesToPlace;
        private const int START_POINT_OFFSET = 12;
        private readonly Random m_random;

        public DiffusionLimitedAggregatoinRoomGenerator(in Random random, in int tilesToPlace)
        {
            m_tilesToPlace = tilesToPlace;
            m_randomWalk = new RandomWalk(random, new List<Vector2Int>() {
                Vector2Int.left,
                Vector2Int.right,
                Vector2Int.up,
                Vector2Int.down,
                new Vector2Int(1, 1),
                new Vector2Int(1, -1),
                new Vector2Int(-1, -1),
                new Vector2Int(-1, 1)
            });

            m_random = random;
        }

        public void GenerateRoom(in RoomModel roomModel)
        {
            int tilesToPlace = m_tilesToPlace;

            int localLeftBorder = roomModel.LeftMostTile.x - START_POINT_OFFSET;
            int localRightBorder = roomModel.RightMostTile.x + START_POINT_OFFSET;
            int localBottomBorder = roomModel.BottomMostTile.y - START_POINT_OFFSET;
            int localTopBorder = roomModel.TopMostTile.y + START_POINT_OFFSET;

            localLeftBorder = Mathf.Max(localLeftBorder, roomModel.LeftBorder);
            localRightBorder = Mathf.Min(localRightBorder, roomModel.RightBorder);
            localBottomBorder = Mathf.Max(localBottomBorder, roomModel.BottomBorder);
            localTopBorder = Mathf.Min(localTopBorder, roomModel.TopBorder);

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

                Vector2Int randomWalkStartPoint = startPoints[m_random.Next(startPoints.Count)];
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

                    if (!roomModel.ContainsTileAtPosition(currentPosition) && roomModel.ContainsTileAtPosition(newPosition))
                        hitTile = true;
                    else
                        currentPosition = newPosition;
                }

                roomModel.AddTile(currentPosition, TileType.Floor);
                --tilesToPlace;

                if (currentPosition.x - START_POINT_OFFSET < localLeftBorder)
                    localLeftBorder = Mathf.Max(currentPosition.x - START_POINT_OFFSET, roomModel.LeftBorder);
                else if (currentPosition.x + START_POINT_OFFSET > localRightBorder)
                    localRightBorder = Mathf.Min(currentPosition.x + START_POINT_OFFSET, roomModel.RightBorder);
                
                if (currentPosition.y - START_POINT_OFFSET < localBottomBorder)
                    localBottomBorder = Mathf.Max(currentPosition.y - START_POINT_OFFSET, roomModel.BottomBorder);
                else if (currentPosition.y + START_POINT_OFFSET > localTopBorder)
                    localTopBorder = Mathf.Min(currentPosition.y + START_POINT_OFFSET, roomModel.TopBorder);
            }
        }
    }
}