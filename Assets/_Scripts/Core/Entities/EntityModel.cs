using MagmaHeart.AI;
using MagmaHeart.Collections;
using MagmaHeart.Core.Entities.Properties;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class EntityModel : AIUnit
    {
        public Transform Transform { get; init; }
        public Health Health { get; init; }
        public Energy Energy { get; init; }
        public EntityData Data { get; init; }
        public EntityStats Stats => Data.Stats;

        public EntityModel(EntityData data, Transform transform, bool isPlayer)
        {
            Data = data;
            IsPlayer = isPlayer;
            PossibleActions = new TypeMap<MagmaHeart.AI.Action>();

            Transform = transform;
            Health = new Health(Stats.MaxHealth);
            Energy = new Energy(Stats.MaxEnergy, Stats.EnergyRegenerationPerTurn);
        }

        public override TypeMap<PropertySnapshot> GetPropertySnapshots()
        {
            TypeMap<PropertySnapshot> properties = base.GetPropertySnapshots();

            properties.Add(new HealthProperty(Health.CurrentHealth, Health.MaxHealth));
            properties.Add(new EnergyProperty(Energy.CurrentEnergy));
            properties.Add(new PositionProperty(Transform.position.ToVector3Int()));

            return properties;
        }
    }
}