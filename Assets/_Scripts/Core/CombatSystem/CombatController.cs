using MagmaHeart.Collections;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using System;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Entities.CombatSystem
{
    public abstract class CombatController
    {
        public Entity Entity { get; init; }
        public Room CurrentRoom => CurrentCombatBoardState.Board;
        public CombatBoardState CurrentCombatBoardState { get; private set; }
        protected CircularList<Entity> CurrentTurnOrder { get; private set; }
        protected MovementAction m_movementAction { get; init; }

        private TaskCompletionSource<bool> m_turnFinished;

        public CombatController(Entity entity)
        {
            Entity = entity;
            m_movementAction = Entity.Model.PossibleActions.Get<MovementAction>();
        }

        public virtual void StartBattle(CombatBoardState combatBoardState, CircularList<Entity> turnOrder)
        {
            CurrentCombatBoardState = combatBoardState;
            CurrentTurnOrder = turnOrder;
        }

        public virtual void EndBattle()
        {
            Entity.Energy.Reset();
            CurrentCombatBoardState = null;
        }

        public virtual Task StartTurn()
        {
            Entity.Energy.Regenerate();

            m_turnFinished = new TaskCompletionSource<bool>();
            return m_turnFinished.Task;
        }

        public virtual void EndTurn()
        {
            m_turnFinished.SetResult(true);
        }
    }
}