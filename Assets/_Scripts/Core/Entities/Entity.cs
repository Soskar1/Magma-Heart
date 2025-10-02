using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class Entity
    {
        public Transform Transform { get; private set; }
        public Health Health { get; private set; }
        public Energy Energy { get; private set; }
        public EntityData Data { get; private set; }
        public EntityStats Stats => Data.Stats;

        public Entity(EntityData data, Transform transform)
        {
            Data = data;
            Transform = transform;

            Health = new Health(Stats.MaxHealth);
            Energy = new Energy(Stats.MaxEnergy, Stats.EnergyRegenerationPerTurn);
        }

        public void Reset() => Health.Reset();
    }
}