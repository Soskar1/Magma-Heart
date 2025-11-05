using MagmaHeart.AI.Reasoning;
using MagmaHeart.Collections;
using MagmaHeart.Core.Entities.Properties;

namespace MagmaHeart.Core.Entities
{
    public class EntityModel : AIUnit
    {
        public Health Health { get; init; }
        public Energy Energy { get; init; }
        public EntityData Data { get; init; }
        public EntityStats Stats => Data.Stats;

        public EntityModel(EntityData data, bool isPlayer)
        {
            Data = data;
            IsPlayer = isPlayer;
            PossibleActions = new TypeMap<AI.Action>();

            Health = new Health(Stats.MaxHealth);
            Energy = new Energy(Stats.MaxEnergy, Stats.EnergyRegenerationPerTurn);
        }

        public override PropertyList GetPropertySnapshots()
        {
            PropertyList properties = base.GetPropertySnapshots();

            properties.Add(new HealthProperty(Health.CurrentHealth, Health.MaxHealth));
            properties.Add(new EnergyProperty(Energy.CurrentEnergy));

            return properties;
        }
    }
}