using MagmaHeart.AI;
using MagmaHeart.Core.Dungeon;
using UnityEngine;

namespace MagmaHeart.Core.Entities.Properties
{
    public record PositionPropertySnapshot(Vector3Int Position) : PropertySnapshot
    {
        public int ManhattanDistance(PositionPropertySnapshot other) => DungeonGrid.ManhattanDistance(Position, other.Position);   
    }
}
