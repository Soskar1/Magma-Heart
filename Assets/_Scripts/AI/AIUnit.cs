using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using MagmaHeart.Collections;

namespace MagmaHeart.AI
{
    public record AIUnit
    {
        public bool IsPlayer { get; init; }
        public TypeMap<UnitAction> PossibleActions { get; init; }

        public AIUnit(bool isPlayer)
        {
            IsPlayer = isPlayer;
            PossibleActions = new TypeMap<UnitAction>();
        }

        public virtual TypeMap<PropertySnapshot> GetPropertySnapshots()
        {
            TypeMap<PropertySnapshot> properties = new TypeMap<PropertySnapshot>();
            IsAlivePropertySnapshot isAliveProperty = new IsAlivePropertySnapshot(true);

            properties.Add(isAliveProperty);

            return properties;
        }
    }
}
