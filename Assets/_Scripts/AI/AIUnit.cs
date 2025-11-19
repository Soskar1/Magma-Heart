using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using MagmaHeart.Collections;

namespace MagmaHeart.AI
{
    public record AIUnit(bool IsPlayer, TypeMap<UnitAction> PossibleActions)
    {
        public virtual TypeMap<PropertySnapshot> GetPropertySnapshots()
        {
            TypeMap<PropertySnapshot> properties = new TypeMap<PropertySnapshot>();
            IsAlivePropertySnapshot isAliveProperty = new IsAlivePropertySnapshot(true);

            properties.Add(isAliveProperty);

            return properties;
        }
    }
}
