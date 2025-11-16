using System;
using MagmaHeart.AI;
using MagmaHeart.AI.States;
using MagmaHeart.Collections;
using MagmaHeart.Core.Entities.Properties;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class EntityModel : AIUnit
    {
        public Func<Vector3Int> GetCurrentTilePosition { get; init; }
        public Health Health { get; init; }
        public Energy Energy { get; init; }
        public EntityData Data { get; init; }
        public EntityStats Stats => Data.Stats;

        public EntityModel(EntityData data, Func<Vector3Int> getCurrentTilePosition, bool isPlayer) : base()
        {
            Data = data;
            IsPlayer = isPlayer;

            GetCurrentTilePosition = getCurrentTilePosition;
            Health = new Health(this, Stats.MaxHealth);
            Energy = new Energy(Stats.MaxEnergy, Stats.EnergyRegenerationPerTurn);
        }

        public override TypeMap<PropertySnapshot> GetPropertySnapshots()
        {
            TypeMap<PropertySnapshot> properties = base.GetPropertySnapshots();

            properties.Add(new HealthPropertySnapshot(Health.CurrentHealth, Health.MaxHealth));
            properties.Add(new EnergyPropertySnapshot(Energy.CurrentEnergy));
            properties.Add(new PositionPropertySnapshot(GetCurrentTilePosition()));

            return properties;
        }
    }
}