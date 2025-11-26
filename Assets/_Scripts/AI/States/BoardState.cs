using MagmaHeart.AI.Boards;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.AI.States
{
    public abstract class BoardState
    {
        public Board Board { get; init; }

        public BoardState(Board board)
        {
            Board = board;
        }

        public abstract T GetProperty<T>(AIUnitModel unit) where T : PropertySnapshot;
        internal virtual async Task ApplyStateChangesAsync(IEnumerable<StateChange> stateChanges, CancellationToken cancellationToken)
        {
            foreach (StateChange change in stateChanges)
                await change.ApplyToAsync(this, cancellationToken);
        }

        internal virtual void ApplyStateChanges(IEnumerable<StateChange> stateChanges)
        {
            foreach (StateChange change in stateChanges)
                change.ApplyTo(this);
        }

        public virtual void AddUnit(Vector2 position, AIUnitModel unit) => Board.AddUnit(position, unit);
        public virtual void RemoveUnit(Vector2 position, AIUnitModel unit) => Board.RemoveUnit(position, unit);
        public virtual void UpdateBoardNodeType(Vector2 position, BoardNodeType newNodeType) => Board.ChangeNodeType(position, newNodeType);
    }
}