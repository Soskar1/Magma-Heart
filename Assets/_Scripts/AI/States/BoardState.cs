using MagmaHeart.AI.Boards;
using System.Collections.Generic;
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

        public abstract T GetProperty<T>(AIUnit unit) where T : PropertySnapshot;
        public virtual void ApplyStateChanges(IEnumerable<StateChange> stateChanges)
        {
            foreach (StateChange change in stateChanges)
                change.ApplyTo(this);
        }

        public virtual void AddUnit(Vector2 position, AIUnit unit) => Board.AddUnit(position, unit);
        public virtual void RemoveUnit(Vector2 position, AIUnit unit) => Board.RemoveUnit(position, unit);
        public virtual void UpdateBoardNodeType(Vector2 position, BoardNodeType newNodeType) => Board.ChangeNodeType(position, newNodeType);
    }
}