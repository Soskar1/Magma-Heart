using MagmaHeart.Abilities;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI
{
    public record AIUnitModel
    {
        public int Id { get; init; }
        public bool IsPlayer { get; init; }
        public bool IsDisabled { get; set; } = false;
        public Dictionary<string, AbilityDefinition> Abilities { get; init; }
        public IDictionary<ParameterId, IParameter> Parameters { get; init; }

        public AIUnitModel(bool isPlayer, int id)
        {
            IsPlayer = isPlayer;
            Id = id;
            Abilities = new Dictionary<string, AbilityDefinition>();
            Parameters = new Dictionary<ParameterId, IParameter>();
        }

        public virtual AIUnitModel DeepCopy()
        {
            return new AIUnitModel(IsPlayer, Id)
            {
                Abilities = new Dictionary<string, AbilityDefinition>(Abilities),
                Parameters = new Dictionary<ParameterId, IParameter>(Parameters)
            };
        }

        public IParameter GetParameter(ParameterId parameterId)
        {
            if (!Parameters.TryGetValue(parameterId, out IParameter parameter))
            {
                Debug.LogWarning($"Parameter {parameterId} not found");
                return null;
            }

            return parameter;
        }
    }
}
