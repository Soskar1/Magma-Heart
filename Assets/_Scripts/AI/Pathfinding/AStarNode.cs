using UnityEngine;

namespace MagmaHeart.AI.Pathifinding
{
    public class AStarNode
    {
        public AStarNodeType Type { get; set; }
        public Vector2 Position { get; init; }

        public AStarNode(Vector2 position, AStarNodeType type)
        {
            Type = type;
            Position = position;
        }
    }
}