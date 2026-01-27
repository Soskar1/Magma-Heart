using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using MagmaHeart.Collections;

namespace MagmaHeart.AI
{
    public record AIUnitModel
    {
        public bool IsPlayer { get; init; }
        public int Id { get; init; }
        public TypeMap<ActionData> PossibleActions { get; init; }

        public AIUnitModel(bool isPlayer, int id)
        {
            IsPlayer = isPlayer;
            Id = id;
            PossibleActions = new TypeMap<ActionData>();
        }

        public virtual TypeMap<PropertySnapshot> GetPropertySnapshots()
        {
            TypeMap<PropertySnapshot> properties = new TypeMap<PropertySnapshot>();
            IsAlivePropertySnapshot isAliveProperty = new IsAlivePropertySnapshot(true);

            properties.Add(isAliveProperty);

            return properties;
        }

        public virtual AIUnitModel DeepCopy()
        {
            return new AIUnitModel(IsPlayer, Id)
            {
                PossibleActions = PossibleActions.DeepCopy()
            };
        }
    }
}
