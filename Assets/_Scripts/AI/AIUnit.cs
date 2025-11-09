using MagmaHeart.Collections;

namespace MagmaHeart.AI
{
    public class AIUnit
    {
        public bool IsPlayer { get; init; }
        public TypeMap<Action> PossibleActions { get; init; }

        public virtual TypeMap<PropertySnapshot> GetPropertySnapshots()
        {
            TypeMap<PropertySnapshot> properties = new TypeMap<PropertySnapshot>();
            IsAlivePropertySnapshot isAliveProperty = new IsAlivePropertySnapshot(true);

            properties.Add(isAliveProperty);

            return properties;
        }
    }
}
