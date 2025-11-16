using MagmaHeart.AI;
using MagmaHeart.AI.Actions;

namespace MagmaHeart.Core.CombatSystem
{
    public abstract class CombatAction<T> : Action<T> where T : ActionArgs
    {
        public CombatAction(AIUnit actionPossessor) : base(actionPossessor) { }

        public abstract int GetEnergyCost(T args);
    }
}
