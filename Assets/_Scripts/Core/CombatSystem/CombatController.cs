using MagmaHeart.Core.Dungeon;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Entities.CombatSystem
{
    public abstract class CombatController
    {
        public Entity Entity { get; init; }
        public Room CurrentRoom { get; private set; }

        private TaskCompletionSource<bool> m_turnFinished;

        public CombatController(Entity entity)
        {
            Entity = entity;
        }

        public virtual void StartBattle(Room room)
        {
            CurrentRoom = room;
        }

        public virtual void EndBattle()
        {
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
            m_turnFinished.SetResult(true);
        }
    }
}