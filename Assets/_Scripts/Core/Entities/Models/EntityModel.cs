using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Resources;
using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.Core.Entities.Models;
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

        public EntityModel(EntityData data, Vector3Int startTilePosition, bool isPlayer, int id) : base(isPlayer, id)
        {
            Stats = data.Stats;
            Data = data;

            TilePosition = startTilePosition;
            Health = new HealthModel(Stats.MaxHealth);
            Energy = new EnergyModel(Stats.MaxEnergy, Stats.EnergyRegenerationPerTurn);
            Strength = new StrengthModel(Stats.Strength);
            Speed = new SpeedModel(Stats.Speed);

            foreach (ActionData actionData in data.Actions)
                PossibleActions.Add(actionData.GetType(), actionData);

            foreach (AbilityDefinition ability in data.Abilities)
                Abilities.Add(ability.Id, ability);
        }

        public override AIUnitModel DeepCopy()
        {
            EntityModel copy = new EntityModel(Data, TilePosition, IsPlayer, Id)
            {
                PossibleActions = PossibleActions.DeepCopy(),
                Health = Health.DeepCopy(),
                Energy = Energy.DeepCopy(),
                Strength = Strength.DeepCopy(),
                Speed = Speed.DeepCopy()
            };
            return copy;
        }

        public float GetResource(ResourceId resource)
        {
            switch (resource.Id)
            {
                case "Health":
                    return Health.CurrentHealth;
                case "Energy":
                    return Energy.CurrentEnergy;
                case "Strength":
                    return Strength.CurrentStrength;
                case "Speed":
                    return Speed.CurrentSpeed;
                default:
                    Debug.LogError($"Resource with id {resource.Id} not found.");
                    return 0;
            }
        }
    }
}