using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class AttackAction : Action<AttackActionArgs>
    {
        public float Damage { get; init; }

        public AttackAction(AIUnit actionPossessor, float damage) : base(actionPossessor)
        {
            Damage = damage;
        }

        public override ActionArgs CreateActionArgs(StateSnapshot state, SimulatedBoard board, AIUnit unit) => new AttackActionArgs(unit);

        public override void Execute(AttackActionArgs args) { }

        public override bool CanSimulate(StateSnapshot state, SimulatedBoard board, AttackActionArgs args)
        {
            Position possessorPosition = state.GetProperty<Position>(ActionPossessor);
            Position targetPosition = state.GetProperty<Position>(args.Target);

            if (possessorPosition.Distance(targetPosition) > 1)
                return false;

            return true;
        }

        public override StateSnapshot Simulate(StateSnapshot state, SimulatedBoard board, AttackActionArgs args)
        {
            StateSnapshot newState = base.Simulate(state, board, args);

            Health targetHealth = state.GetProperty<Health>(args.Target);

            if (targetHealth.CurrentHealth < Damage)
                newState.Update(args.Target, new IsAlivePropertySnapshot(false));

            targetHealth = new Health(targetHealth.CurrentHealth - Damage, targetHealth.MaxHealth);
            newState.Update(args.Target, targetHealth);

            return newState;
        }

        public override bool CanSimulate(StateSnapshot state, SimulatedBoard board, ActionArgs args) => CanSimulate(state, board, (AttackActionArgs)args);
        public override void Execute(ActionArgs args) => Execute((AttackActionArgs)args);
    }
}
