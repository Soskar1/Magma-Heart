using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.Dungeon;
using UnityEngine;

namespace MagmaHeart.Core.Entities.Properties
{
    public record PositionProperty(Vector3Int Position) : PropertySnapshot
    {
        public int ManhattanDistance(PositionProperty other) => DungeonGrid.ManhattanDistance(Position, other.Position);   
    }
}
