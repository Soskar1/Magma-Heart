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
        public MagmaHeartModel MagmaHeart { get; init; }
        public EntityStats Stats { get; init; }
        public EntityData Data { get; init; }

        public AbilityDefinition MovementAbility { get; init; }
        public AbilityDefinition AttackAbility { get; init; }

        public EntityModel(EntityData data, Vector3Int startTilePosition, bool isPlayer, int id) : base(isPlayer, id, data.Plans)
        {
            Stats = data.Stats;
            Data = data;

            TilePosition = startTilePosition;
            Health = new HealthModel(Stats.MaxHealth, data.ParameterDatabase?.Health);
            Energy = new EnergyModel(data.ParameterDatabase?.Energy, Stats.MaxEnergy, Stats.EnergyRegenerationPerTurn);
            Strength = new StrengthModel(Stats.Strength, data.ParameterDatabase?.Strength);
            Speed = new SpeedModel(Stats.Speed, data.ParameterDatabase?.Speed);
            MagmaHeart = new MagmaHeartModel(data.ParameterDatabase?.MagmaHeart);
            IsDisabled = () => Health.CurrentHealth <= 0;

            if (data.AttackAbility != null)
            {
                AttackAbility = data.AttackAbility;
                Abilities.Add(AttackAbility.Id, AttackAbility);
            }

            if (data.MovementAbility != null)
            {
                MovementAbility = data.MovementAbility;
                Abilities.Add(MovementAbility.Id, MovementAbility);
            }

            if (data.ParameterDatabase != null)
            {
                Parameters.Add(data.ParameterDatabase.Energy, Energy);
                Parameters.Add(data.ParameterDatabase.Strength, Strength);
                Parameters.Add(data.ParameterDatabase.Speed, Speed);
                Parameters.Add(data.ParameterDatabase.Health, Health);
            }
        }

        public override AIUnitModel DeepCopy()
        {
            HealthModel healthCopy = Health.DeepCopy();
            EnergyModel energyCopy = Energy.DeepCopy();
            StrengthModel strengthCopy = Strength.DeepCopy();
            SpeedModel speedCopy = Speed.DeepCopy();

            EntityModel copy = new EntityModel(Data, TilePosition, IsPlayer, Id)
            {
                Abilities = new Dictionary<string, AbilityDefinition>(Abilities),
                Cooldowns = new Dictionary<string, int>(Cooldowns),
                Health = healthCopy,
                Energy = energyCopy,
                Strength = strengthCopy,
                Speed = speedCopy
            };

            if (Data.ParameterDatabase != null)
            {
                copy.Parameters[Data.ParameterDatabase.Health] = healthCopy;
                copy.Parameters[Data.ParameterDatabase.Energy] = energyCopy;
                copy.Parameters[Data.ParameterDatabase.Strength] = strengthCopy;
                copy.Parameters[Data.ParameterDatabase.Speed] = speedCopy;
            }

            return copy;
        }
    }
}