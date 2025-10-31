using MagmaHeart.AI.Reasoning;
using System.Linq;

namespace MagmaHeart.AI
{
    public abstract class Action
    {
        public AIUnit ActionPossessor { get; }

        public Action(AIUnit actionPossessor) => ActionPossessor = actionPossessor;

        public abstract bool CanSimulate(StateSnapshot state, AIUnit target);
        public virtual StateSnapshot Simulate(StateSnapshot state, AIUnit target)
        {
            StateSnapshot newState = state with
            {
                StateProperties = state.StateProperties.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.DeepCopy())
            };

            return newState;
        }

        public abstract void Execute();
    }
}
