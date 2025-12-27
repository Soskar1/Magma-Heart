using System.Collections.Generic;
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

            AddWalls(roomModel);

            return roomModel;
        }

        private void AddWalls(RoomModel roomModel)
        {
            HashSet<DungeonTile> outline = TileOutline.GetOutline(roomModel.GetTiles());

            foreach (DungeonTile tile in outline)
                tile.Type = TileType.Wall;
        }
    }
}

