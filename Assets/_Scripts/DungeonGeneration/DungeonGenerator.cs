using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.DungeonGeneration
{
    public class DungeonGenerator
    {
        private readonly DungeonGeneratorData m_data;

        public DungeonGenerator(DungeonGeneratorData data) => m_data = data;

        public RoomModel GenerateRoom()
        {
            Vector2Int roomSpaceSize = m_data.RoomSpaceSize;
            BoundsInt roomSpace = new BoundsInt(Vector3Int.zero, new Vector3Int(roomSpaceSize.x, roomSpaceSize.y, 0));

            RoomModel roomModel = new RoomModel(roomSpace);

            foreach (IRoomGenerator generator in m_data.Generators)
                generator.GenerateRoom(roomModel);

            HashSet<DungeonTile> outline = TileOutline.GetOutline(roomModel.GetTiles());
            AddWalls(roomModel, outline);
            AddDoors(roomModel, outline);

            return roomModel;
        }

        private void AddWalls(RoomModel roomModel, HashSet<DungeonTile> outline)
        {
            foreach (DungeonTile tile in outline)
                tile.Type = TileType.Wall;
        }

        private void AddDoors(RoomModel roomModel, HashSet<DungeonTile> outline)
        {
            IEnumerable<DungeonTile> leftSideTiles = outline
                .Where(tile => tile.Position.x < roomModel.WorldPosition.x)
                .OrderBy(tile => Mathf.Abs(tile.Position.y - roomModel.WorldPosition.y));

            IEnumerable<DungeonTile> rightSideTiles = outline
                .Where(tile => tile.Position.x > roomModel.WorldPosition.x)
                .OrderBy(tile => Mathf.Abs(tile.Position.y - roomModel.WorldPosition.y));

            roomModel.EntranceDoor = GetDoorTile(roomModel, leftSideTiles);
            roomModel.ExitDoor = GetDoorTile(roomModel, rightSideTiles);
        }

        private DungeonTile GetDoorTile(RoomModel roomModel, IEnumerable<DungeonTile> tiles)
        {
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            foreach (DungeonTile tile in tiles)
                foreach (DungeonTile adjacentTile in roomModel.GetAdjacentTiles(tile))
                    if (adjacentTile.Type == TileType.Floor)
                        return tile;

            return null;
        }
    }
}

