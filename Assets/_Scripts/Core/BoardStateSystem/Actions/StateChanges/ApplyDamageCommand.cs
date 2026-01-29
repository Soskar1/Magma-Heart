using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public record ApplyDamageCommand(int TargetId, float Damage) : IBoardCommand
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
