using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class Entity : AIUnit
    {
        private float m_health;
        public Vector2 Position { get; private set; }

        public Entity(float health, Vector2 position, bool isPlayer)
        {
            m_health = health;
            Position = position;

            IsPlayer = isPlayer;
            PossibleActions = new Action[] {
                    new AttackAction(this, 4),
                    new MoveAction(this, 3),
                    new EngageAction(this, 4, 1)
                };
        }

        public override Dictionary<Type, PropertySnapshot> GetPropertySnapshots()
        {
            Dictionary<Type, PropertySnapshot> properties = base.GetPropertySnapshots();
            Health healthLeft = new Health(m_health);
            Position position = new Position(Position);

            properties.Add(typeof(Health), healthLeft);
            properties.Add(typeof(Position), position);

            return properties;
        }
    }
}
