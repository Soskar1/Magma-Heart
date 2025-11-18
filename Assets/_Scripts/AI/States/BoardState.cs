using MagmaHeart.AI.Boards;
using System.Collections.Generic;

namespace MagmaHeart.AI.States
{
    public abstract class BoardState
    {
        public Board Board { get; init; }

        public BoardState(Board board)
        {
            Board = board;
        }

        public abstract T GetProperty<T>(AIUnit unit) where T : PropertySnapshot;
        public virtual void ApplyStateChanges(List<StateChange> stateChanges)
        {
            foreach (StateChange change in stateChanges)
                change.ApplyTo(this);
        }
    }
}