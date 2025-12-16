using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using MagmaHeart.Collections;
using System.Collections.Generic;

namespace MagmaHeart.AI
{
    public record AIUnitModel
    {
        public bool IsPlayer { get; init; }
        public List<ActionDefinition> PossibleActions { get; init; }
        public TypeMap<ActionData> PossibleActionDatas { get; init; }

        public AIUnitModel(bool isPlayer)
        {
            IsPlayer = isPlayer;
            PossibleActions = new List<ActionDefinition>();
            PossibleActionDatas = new TypeMap<ActionData>();
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
