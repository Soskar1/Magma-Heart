using UnityEngine;

namespace MagmaHeart.AI.Boards
{
    public class BoardNode
    {
        public BoardNodeType Type { get; set; }
        public Vector2 Position { get; init; }

        public BoardNode(Vector2 position, BoardNodeType type)
        {
            Type = type;
            Position = position;
        }
    }
}