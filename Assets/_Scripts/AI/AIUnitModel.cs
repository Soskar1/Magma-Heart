using MagmaHeart.Abilities;
using MagmaHeart.AI.Reasoning.Plans;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI
{
    public record AIUnitModel
    {
        public int Id { get; init; }
        public bool IsPlayer { get; init; }
        public Func<bool> IsDisabled { get; init; }
        public Dictionary<string, AbilityDefinition> Abilities { get; init; }
        public IDictionary<ParameterId, IParameter> Parameters { get; init; }
        public IReadOnlyList<PlanDefinition> Plans { get; init; }

        public AIUnitModel(bool isPlayer, int id, IReadOnlyList<PlanDefinition> plans)
        {
            IsPlayer = isPlayer;
            Id = id;
            Abilities = new Dictionary<string, AbilityDefinition>();
            Parameters = new Dictionary<ParameterId, IParameter>();
            Plans = plans;
            IsDisabled = () => false;
        }

        public virtual AIUnitModel DeepCopy()
        {
            return new AIUnitModel(IsPlayer, Id, Plans)
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
