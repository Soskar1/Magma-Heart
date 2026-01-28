using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record ApplyDamageStateChange(int Damage, int TargetId) : StateChange
    {
        public override void ApplyChangeToSimulation(SimulatedBoardState simulation)
        {
            if (simulation.Board.TryGetUnit(TargetId, out Entity targetUnit))
            {
                targetUnit.CurrentHealth -= Damage;

                if (targetUnit.CurrentHealth <= 0)
                    targetUnit.IsDisabled = true;
            }
        }

        public override void UndoChangeToSimulation(SimulatedBoardState simulation)
        {
            if (simulation.Board.TryGetUnit(TargetId, out Entity targetUnit))
            {
                targetUnit.CurrentHealth += Damage;

                if (targetUnit.CurrentHealth > 0)
                    targetUnit.IsDisabled = false;
            }
        }
    }
}
