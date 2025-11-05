using MagmaHeart.Collections;

namespace MagmaHeart.AI.Reasoning
{
    public class AIUnit
    {
        public bool IsPlayer { get; init; }
        public TypeMap<Action> PossibleActions { get; init; }

        public virtual TypeMap<PropertySnapshot> GetPropertySnapshots()
        {
            TypeMap<PropertySnapshot> properties = new TypeMap<PropertySnapshot>();
            IsAliveProperty isAliveProperty = new IsAliveProperty(true);

            properties.Add(isAliveProperty);

            return properties;
        }
    }
}
