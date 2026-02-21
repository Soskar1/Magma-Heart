using MagmaHeart.Abilities;
using MagmaHeart.AI.Actions;
using MagmaHeart.Collections;
using System.Collections.Generic;

namespace MagmaHeart.AI
{
    public record AIUnitModel
    {
        public int Id { get; init; }
        public bool IsPlayer { get; init; }
        public bool IsDisabled { get; set; } = false;
        public TypeMap<ActionData> PossibleActions { get; init; }
        public Dictionary<string, AbilityDefinition> Abilities { get; init; }

        public AIUnitModel(bool isPlayer, int id)
        {
            IsPlayer = isPlayer;
            Id = id;
            PossibleActions = new TypeMap<ActionData>();
            Abilities = new Dictionary<string, AbilityDefinition>();
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
