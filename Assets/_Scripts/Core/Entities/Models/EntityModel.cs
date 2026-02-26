using MagmaHeart.Abilities;
using MagmaHeart.AI;
using MagmaHeart.Core.Entities.Models;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public record EntityModel : AIUnitModel
    {
        public Vector3Int TilePosition { get; set; }
        public HealthModel Health { get; init; }
        public EnergyModel Energy { get; init; }
        public StrengthModel Strength { get; init; }
        public SpeedModel Speed { get; init; }
        public EntityStats Stats { get; init; }
        public EntityData Data { get; init; }

        public AbilityDefinition MovementAbility { get; init; }
        public AbilityDefinition AttackAbility { get; init; }

        private readonly Dictionary<ParameterId, IParameter> m_parameters;

        public EntityModel(EntityData data, Vector3Int startTilePosition, bool isPlayer, int id) : base(isPlayer, id)
        {
            Stats = data.Stats;
            Data = data;

            TilePosition = startTilePosition;
            Health = new HealthModel(Stats.MaxHealth);
            Energy = new EnergyModel(data.ParameterDatabase.Energy, Stats.MaxEnergy, Stats.EnergyRegenerationPerTurn);
            Strength = new StrengthModel(Stats.Strength, data.ParameterDatabase.Strength);
            Speed = new SpeedModel(Stats.Speed, data.ParameterDatabase.Speed);

            AttackAbility = data.AttackAbility;
            MovementAbility = data.MovementAbility;
            Abilities.Add(AttackAbility.Id, AttackAbility);
            Abilities.Add(MovementAbility.Id, MovementAbility);

            foreach (AbilityDefinition ability in data.AdditionalAbilities)
                Abilities.Add(ability.Id, ability);

            m_parameters = new Dictionary<ParameterId, IParameter>()
            {
                { data.ParameterDatabase.Energy, Energy },
                { data.ParameterDatabase.Strength, Strength },
                { data.ParameterDatabase.Speed, Speed  }
            };
        }

        public override AIUnitModel DeepCopy()
        {
            EntityModel copy = new EntityModel(Data, TilePosition, IsPlayer, Id)
            {
                Abilities = new Dictionary<string, AbilityDefinition>(Abilities),
                Health = Health.DeepCopy(),
                Energy = Energy.DeepCopy(),
                Strength = Strength.DeepCopy(),
                Speed = Speed.DeepCopy()
            };
            return copy;
        }

        public IParameter GetParameter(ParameterId parameterId)
        {
            if (!m_parameters.TryGetValue(parameterId, out IParameter parameter))
            {
                Debug.LogWarning($"Parameter {parameterId} not found");
                return null;
            }

            return parameter;
        }
    }
}