using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record ApplyDamageStateChange(int Damage, int TargetId) : IBoardCommand
    {
        public void Execute(Board board)
        {
            board.TryGetUnit(TargetId, out Entity targetUnit);
            targetUnit.CurrentHealth -= Damage;

            if (targetUnit.CurrentHealth <= 0)
                targetUnit.IsDisabled = true;
        }

        public void Undo(Board board)
        {
            board.TryGetUnit(TargetId, out Entity targetUnit);
            targetUnit.CurrentHealth += Damage;

            if (targetUnit.CurrentHealth > 0)
                targetUnit.IsDisabled = false;
        }
    }
}
