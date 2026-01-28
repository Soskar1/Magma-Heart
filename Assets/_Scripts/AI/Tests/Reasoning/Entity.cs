using MagmaHeart.AI.States;
using MagmaHeart.Collections;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record Entity(bool IsPlayer, int id) : AIUnitModel(IsPlayer, id)
    {
        public int CurrentHealth { get; set; }
        public Vector2 Position { get; set; }

        public override TypeMap<PropertySnapshot> GetPropertySnapshots()
        {
            TypeMap<PropertySnapshot> properties = base.GetPropertySnapshots();
            Health healthLeft = new Health(CurrentHealth, CurrentHealth);
            Position position = new Position(Position);

            properties.Add(healthLeft);
            properties.Add(position);

            return properties;
        }

        public override AIUnitModel DeepCopy()
        {
            return new Entity(IsPlayer, Id)
            {
                PossibleActions = PossibleActions.DeepCopy(),
                CurrentHealth = CurrentHealth,
                Position = Position
            };
        }
    }
}
