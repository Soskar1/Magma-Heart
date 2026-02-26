using MagmaHeart.Abilities;
using System.Collections.Generic;

namespace MagmaHeart.AI
{
    public record AIUnitModel
    {
        public int Id { get; init; }
        public bool IsPlayer { get; init; }
        public bool IsDisabled { get; set; } = false;
        public Dictionary<string, AbilityDefinition> Abilities { get; init; }

        public AIUnitModel(bool isPlayer, int id)
        {
            IsPlayer = isPlayer;
            Id = id;
            Abilities = new Dictionary<string, AbilityDefinition>();
        }

        public virtual AIUnitModel DeepCopy()
        {
            return new AIUnitModel(IsPlayer, Id)
            {
                Abilities = new Dictionary<string, AbilityDefinition>(Abilities)
            };
        }
    }
}
