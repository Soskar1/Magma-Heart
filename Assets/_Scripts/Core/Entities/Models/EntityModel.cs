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
        public HealthModel Health { get; init; }
        public EnergyModel Energy { get; init; }
        public EntityStats Stats { get; init; }
        public EntityData Data { get; init; }

        public EntityModel(EntityData data, Func<Vector3Int> getCurrentTilePosition, bool isPlayer) : base(isPlayer)
        {
            Stats = data.Stats;
            Data = data;

            GetCurrentTilePosition = getCurrentTilePosition;
            Health = new HealthModel(Stats.MaxHealth);
            Energy = new EnergyModel(Stats.MaxEnergy);

            foreach (ActionData actionData in data.Actions)
            {
                PossibleActions.Add(actionData.GetDefinition());
                PossibleActionDatas.Add(actionData.GetType(), actionData);
            }
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