using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Execution;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.BoardStateSystem.Actions.Commands
{
    public record ApplyDamageCommand(int ExecutorId, int TargetId, float Damage) : IBoardCommand
    {
        private float m_previosHealth;

        public void Execute(Board board)
        {
            board.TryGetUnit(TargetId, out EntityModel target);
            m_previosHealth = target.Health.CurrentHealth;
            target.Health.CurrentHealth -= Damage;

            if (target.Health.CurrentHealth <= 0)
                target.IsDisabled = true;
        }

        public void Undo(Board board)
        {
            board.TryGetUnit(TargetId, out EntityModel target);
            target.Health.CurrentHealth = m_previosHealth;

            if (target.Health.CurrentHealth > 0)
                target.IsDisabled = false;
        }
    }
}
