using System.Linq;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class AttackAction : IAction
    {
        public float Damage { get; init; }
        public AIUnit ActionPossessor { get; }

        public AttackAction(AIUnit actionPossessor, float damage)
        {
            ActionPossessor = actionPossessor;
            Damage = damage;
        }

        public void Execute() { }

        public bool CanSimulate(StateSnapshot state, AIUnit target)
        {
            Position possessorPosition = state.GetProperty<Position>(ActionPossessor);
            Position targetPosition = state.GetProperty<Position>(target);

            if (possessorPosition.Distance(targetPosition) > 1)
                return false;

            return true;
        }

        public StateSnapshot Simulate(StateSnapshot state, AIUnit target)
        {
            StateSnapshot newState = state with
            {
                StateProperties = state.StateProperties.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.ToDictionary(
                        inner => inner.Key,
                        inner => inner.Value
                    )
                )
            };

            DamageToTarget damageToTarget = new DamageToTarget(Damage);
            newState.Add(ActionPossessor, damageToTarget);

            Health targetHealth = state.GetProperty<Health>(target);
            IsAliveProperty isAliveProperty = null;

            if (targetHealth.Value < Damage)
            {
                newState.Replace(target, new Health(-100));
                newState.Replace(target, new IsAliveProperty(false));
            }
            else
            {
                newState.Replace(target, new Health(targetHealth.Value - Damage));
            }

            if (isAliveProperty != null)
                newState.Replace(target, isAliveProperty);

            return newState;
        }
    }
}
