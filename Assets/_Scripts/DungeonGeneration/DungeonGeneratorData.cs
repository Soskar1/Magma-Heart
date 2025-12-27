using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.DungeonGeneration
{
    public record DungeonGeneratorData(Vector2Int RoomSpaceSize, List<IRoomGenerator> Generators);
}