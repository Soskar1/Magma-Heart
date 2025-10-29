using System.Collections.Generic;

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
            Position possessorPosition = (Position)state.GetProperty(ActionPossessor, typeof(Position));
            Position targetPosition = (Position)state.GetProperty(target, typeof(Position));

            if (possessorPosition.Distance(targetPosition) > 1)
                return false;

            return true;
        }

        public StateSnapshot Simulate(StateSnapshot state, AIUnit target)
        {
            List<PropertySnapshot> propertiesToAdd = new List<PropertySnapshot>();
            DamageToTarget damageToTarget = new DamageToTarget(Damage);
            propertiesToAdd.Add(damageToTarget);

            Health targetHealth = (Health)state.GetProperty(target, typeof(Health));
            IsAliveProperty isAliveProperty = null;

            if (targetHealth.Value < Damage)
            {
                isAliveProperty = new IsAliveProperty(false);
                propertiesToAdd.Add(new Health(-100));
            }
            else
            {
                propertiesToAdd.Add(new Health(targetHealth.Value - Damage));
            }

            StateSnapshot newState = state with
            {
                StateProperties = state.StateProperties
            };

            newState.Add(target, propertiesToAdd);

            if (isAliveProperty != null)
                newState.Replace(target, isAliveProperty);

            return newState;
        }
    }
}
