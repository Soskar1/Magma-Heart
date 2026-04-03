using MagmaHeart.Abilities;
using MagmaHeart.AI.Reasoning.Plans;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public IDictionary<string, int> Cooldowns { get; init; } = new Dictionary<string, int>();

        public event EventHandler<OnCooldownChangedEventArgs> OnCooldownChanged;
        public event EventHandler<bool> OnShouldSkipTurnChanged;

        private bool m_shouldSkipTurn;
        public bool ShouldSkipTurn => m_shouldSkipTurn;

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
                Parameters = new Dictionary<ParameterId, IParameter>(Parameters),
                Cooldowns = new Dictionary<string, int>(Cooldowns),
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

        public IReadOnlyList<string> GetCooldownIds() => Cooldowns.Keys.ToList();
        public int GetCooldown(string abilityId) => Cooldowns.TryGetValue(abilityId, out int turns) ? turns : 0;

        public void SetCooldown(string abilityId, int turns)
        {
            Cooldowns[abilityId] = turns;

            if (turns <= 0)
                Cooldowns.Remove(abilityId);

            OnCooldownChanged?.Invoke(this, new OnCooldownChangedEventArgs(abilityId, turns));
        }

        public void SkipNextTurn()
        {
            m_shouldSkipTurn = true;
            OnShouldSkipTurnChanged.Invoke(this, m_shouldSkipTurn);
        }

        public void AllowNextTurn()
        {
            m_shouldSkipTurn = false;
            OnShouldSkipTurnChanged.Invoke(this, m_shouldSkipTurn);
        }
    }
}
