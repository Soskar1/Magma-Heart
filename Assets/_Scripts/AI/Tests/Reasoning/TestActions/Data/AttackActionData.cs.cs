using MagmaHeart.AI.Actions;

namespace MagmaHeart.AI.Reasoning.Tests
{
    public class AttackActionData : ActionData
    {
        public float Damage { get; init; }
        public AttackActionData(float damage) => Damage = damage;

        public override ActionDefinition GetDefinition()
        {
            return new ActionDefinition(typeof(AttackAction), this, new AttackActionArgumentCreator());
        }
    }    
}
