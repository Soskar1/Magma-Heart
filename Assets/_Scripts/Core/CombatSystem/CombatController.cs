using MagmaHeart.Collections;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Entities.CombatSystem
{
    public abstract class CombatController
    {
        public Entity Entity { get; init; }
        public Room CurrentRoom { get; private set; }
        protected CircularList<Entity> CurrentTurnOrder { get; private set; }
        protected MovementAction m_movementAction { get; init; }

        private TaskCompletionSource<bool> m_turnFinished;

        public CombatController(Entity entity)
        {
            Entity = entity;
            m_movementAction = Entity.Model.PossibleActions.Get<MovementAction>();
        }

        public virtual void StartBattle(Room room, CircularList<Entity> turnOrder)
        {
            CurrentRoom = room;
            CurrentTurnOrder = turnOrder;

            m_movementAction.SetCurrentRoom(CurrentRoom);
        }

        public virtual void EndBattle()
        {
            Entity.Energy.Reset();
            CurrentRoom = null;
        }

        public virtual Task StartTurn()
        {
            Entity.Energy.Regenerate();

            m_turnFinished = new TaskCompletionSource<bool>();
            return m_turnFinished.Task;
        }

        public virtual void EndTurn()
        {
            m_movementAction.Reset();
            m_turnFinished.SetResult(true);
        }
    }
}