using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using System;
using MagmaHeart.DungeonGeneration.RoomGeneration;

namespace MagmaHeart.DungeonGeneration
{
    public class DungeonGenerator
    {
        private readonly Random m_random;
        private readonly DungeonGeneratorData m_data;

        public DungeonGenerator(TextAsset configFile, int seed = -1)
        {
            if (seed == -1)
                seed = Environment.TickCount;

            m_random = new Random(seed);

            DungeonGeneratorDataDeserializer deserializer = new DungeonGeneratorDataDeserializer(configFile, m_random);
            m_data = deserializer.Deserialize();
        }

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

