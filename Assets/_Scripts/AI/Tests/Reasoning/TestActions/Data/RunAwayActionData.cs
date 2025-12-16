using MagmaHeart.AI.Actions;

namespace MagmaHeart.AI.Reasoning.Tests
{
    public class RunAwayActionData : ActionData
    {
        public float Speed { get; init; }

        public RunAwayActionData(float speed)
        {
            Speed = speed;
        }

        public override ActionDefinition GetDefinition()
        {
            return new ActionDefinition(typeof(RunAwayAction), this, new RunAwayActionArgumentCreator(), new EnemyTargetSelector());
        }
    }
}
