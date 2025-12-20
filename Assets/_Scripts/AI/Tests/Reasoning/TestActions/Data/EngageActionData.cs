using MagmaHeart.AI.Actions;

namespace MagmaHeart.AI.Reasoning.Tests
{
    public class EngageActionData : ActionData
    {
        public float Damage { get; init; }
        public float Speed { get; init; }

        public EngageActionData(float damage, float speed)
        {
            Damage = damage;
            Speed = speed;
        }

        public override ActionDefinition GetDefinition()
        {
            return new ActionDefinition(typeof(EngageAction), this, new EngageActionArgumentCreator());
        }
    }
}
