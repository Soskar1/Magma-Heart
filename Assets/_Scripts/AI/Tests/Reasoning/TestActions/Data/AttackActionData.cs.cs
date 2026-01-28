using MagmaHeart.AI.Actions;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class AttackActionData : ActionData
    {
        public int Damage { get; init; }
        public AttackActionData(int damage) => Damage = damage;

        public override ActionDefinition GetDefinition()
        {
            return new ActionDefinition(typeof(AttackAction), this);
        }
    }
}
