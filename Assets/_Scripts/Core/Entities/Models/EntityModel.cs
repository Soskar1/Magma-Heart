using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using MagmaHeart.Collections;
using MagmaHeart.Core.Entities.Models;
using MagmaHeart.Core.Entities.Properties;
using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public record EntityModel : AIUnitModel
    {
        public Func<Vector3Int> GetCurrentTilePosition { get; init; }
        public Vector3Int CurrentTilePosition { get; set; }
        public HealthModel Health { get; init; }
        public EnergyModel Energy { get; init; }
        public StrengthModel Strength { get; init; }
        public SpeedModel Speed { get; init; }
        public EntityStats Stats { get; init; }
        public EntityData Data { get; init; }

        public EntityModel(EntityData data, Func<Vector3Int> getCurrentTilePosition, bool isPlayer, int id) : base(isPlayer, id)
        {
            Stats = data.Stats;
            Data = data;

            GetCurrentTilePosition = getCurrentTilePosition;
            Health = new HealthModel(Stats.MaxHealth);
            Energy = new EnergyModel(Stats.MaxEnergy, Stats.EnergyRegenerationPerTurn);
            Strength = new StrengthModel(Stats.Strength);
            Speed = new SpeedModel(Stats.Speed);

            foreach (ActionData actionData in data.Actions)
                PossibleActions.Add(actionData.GetType(), actionData);
        }

        public override TypeMap<PropertySnapshot> GetPropertySnapshots()
        {
            TypeMap<PropertySnapshot> properties = base.GetPropertySnapshots();

            properties.Add(new HealthPropertySnapshot(Health.CurrentHealth, Health.MaxHealth));
            properties.Add(new EnergyPropertySnapshot(Energy.CurrentEnergy, Energy.MaxEnergy));
            properties.Add(new StrengthPropertySnapshot(Strength.CurrentStrength));
            properties.Add(new PositionPropertySnapshot(GetCurrentTilePosition()));
            properties.Add(new SpeedPropertySnapshot(Speed.CurrentSpeed));

            return properties;
        }

        public override AIUnitModel DeepCopy()
        {
            EntityModel copy = new EntityModel(Data, GetCurrentTilePosition, IsPlayer, Id)
            {
                PossibleActions = PossibleActions.DeepCopy(),
                Health = Health.DeepCopy(),
                Energy = Energy.DeepCopy(),
                Strength = Strength.DeepCopy(),
                Speed = Speed.DeepCopy()
            };
            return copy;
        }
    }
}