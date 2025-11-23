using MagmaHeart.AI;
using MagmaHeart.AI.States;
using MagmaHeart.Collections;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.Entities.Properties;
using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public record EntityModel : AIUnit
    {
        public Func<Vector3Int> GetCurrentTilePosition { get; init; }
        public Health Health { get; init; }
        public Energy Energy { get; init; }
        public EntityStats Stats { get; init; }
        public Entity Entity { get; init; }

        public EntityModel(Entity entity, EntityStats stats, Func<Vector3Int> getCurrentTilePosition, bool isPlayer) : base(isPlayer)
        {
            Entity = entity;
            Stats = stats;

            GetCurrentTilePosition = getCurrentTilePosition;
            Health = new Health(this, Stats.MaxHealth);
            Energy = new Energy(Stats.MaxEnergy);

            PossibleActions.Add(new DoNothingAction(this));
            PossibleActions.Add(new MovementAction(this));
            PossibleActions.Add(new AttackAction(this));
        }

        public override TypeMap<PropertySnapshot> GetPropertySnapshots()
        {
            TypeMap<PropertySnapshot> properties = base.GetPropertySnapshots();

            properties.Add(new HealthPropertySnapshot(Health.CurrentHealth, Health.MaxHealth));
            properties.Add(new EnergyPropertySnapshot(Energy.CurrentEnergy, Energy.MaxEnergy));
            properties.Add(new PositionPropertySnapshot(GetCurrentTilePosition()));

            return properties;
        }
    }
}