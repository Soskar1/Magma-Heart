using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using MagmaHeart.Collections;
using System.Collections.Generic;

namespace MagmaHeart.AI
{
    public record AIUnitModel
    {
        public bool IsPlayer { get; init; }
        public TypeMap<UnitAction> PossibleActions { get; init; }
        public List<ActionEntry> PossibleActionEntries { get; init; }

        public AIUnitModel(bool isPlayer)
        {
            IsPlayer = isPlayer;
            PossibleActions = new TypeMap<UnitAction>();
            PossibleActionEntries = new List<ActionEntry>();
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
