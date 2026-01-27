using MagmaHeart.AI.States;
using MagmaHeart.Collections;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record Entity(float Health, Vector2 Position, bool IsPlayer, int id) : AIUnitModel(IsPlayer, id)
    {
        public override TypeMap<PropertySnapshot> GetPropertySnapshots()
        {
            TypeMap<PropertySnapshot> properties = base.GetPropertySnapshots();
            Health healthLeft = new Health(Health, Health);
            Position position = new Position(Position);

            properties.Add(healthLeft);
            properties.Add(position);

            return properties;
        }
    }
}
