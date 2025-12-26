using MagmaHeart.AI.Actions;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class MoveActionData : ActionData
    {
        public float Speed { get; init; }
        public MoveActionData(float speed) => Speed = speed;

        public override ActionDefinition GetDefinition() => new ActionDefinition(typeof(MoveAction), this, new MoveActionResolver());
    }
}
