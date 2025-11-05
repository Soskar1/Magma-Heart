using MagmaHeart.Collections;

namespace MagmaHeart.AI.Reasoning
{
    public class AIUnit
    {
        public bool IsPlayer { get; init; }
        public TypeMap<Action> PossibleActions { get; init; }

        public virtual PropertyList GetPropertySnapshots()
        {
            PropertyList list = new PropertyList();
            IsAliveProperty isAliveProperty = new IsAliveProperty(true);

            list.Add(isAliveProperty);

            return list;
        }
    }
}
