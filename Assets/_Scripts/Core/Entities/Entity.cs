using MagmaHeart.AI;
using MagmaHeart.AI.Reasoning;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class Entity : AIUnit
    {
        public Transform Transform { get; init; }
        public Health Health { get; init; }
        public Energy Energy { get; init; }
        public EntityData Data { get; init; }
        public EntityStats Stats => Data.Stats;

        public Entity(EntityData data, Transform transform, bool isPlayer)
        {
            Data = data;
            IsPlayer = isPlayer;
            PossibleActions = new HashSet<Action>();

            Transform = transform;
            Health = new Health(Stats.MaxHealth);
            Energy = new Energy(Stats.MaxEnergy, Stats.EnergyRegenerationPerTurn);
        }

        public void Reset() => Health.Reset();
    }
}