namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class AttackAction : Action
    {
        public float Damage { get; init; }

        public AttackAction(AIUnit actionPossessor, float damage) : base(actionPossessor)
        {
            Damage = damage;
        }

        public override void Execute() { }

        public override bool CanSimulate(StateSnapshot state, AIUnit target)
        {
            Position possessorPosition = state.GetProperty<Position>(ActionPossessor);
            Position targetPosition = state.GetProperty<Position>(target);

            if (possessorPosition.Distance(targetPosition) > 1)
                return false;

            return true;
        }

        public override StateSnapshot Simulate(StateSnapshot state, AIUnit target)
        {
            StateSnapshot newState = base.Simulate(state, target);

            Health targetHealth = state.GetProperty<Health>(target);

            if (targetHealth.Value < Damage)
                newState.Replace(target, new IsAliveProperty(false));

            DamageToTarget damageToTarget = new DamageToTarget(Damage);
            newState.Add(ActionPossessor, damageToTarget);
            newState.Replace(target, new Health(targetHealth.Value - Damage));

            return newState;
        }
    }
}
