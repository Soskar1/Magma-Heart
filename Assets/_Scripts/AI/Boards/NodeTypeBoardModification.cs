using UnityEngine;

namespace MagmaHeart.AI.Boards
{
    public record NodeTypeBoardModification(Vector2 Position, BoardNodeType Type) : BoardModification
    {
        private BoardNodeType m_previousType;

        public override void Apply(SimulatedBoard board)
        {
            m_previousType = board.Graph.GetNode(Position).Type;
            board.Graph.ChangeNodeType(Position, Type);
        }

        public override void Undo(SimulatedBoard board) => board.Graph.ChangeNodeType(Position, m_previousType);
    }
}
