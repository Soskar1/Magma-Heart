using System;
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
                    new EngageAction(this, 4, 1),
                    new RunAwayAction(this, 3)
                };
        }

        public override PropertyList GetPropertySnapshots()
        {
            PropertyList properties = base.GetPropertySnapshots();
            Health healthLeft = new Health(m_health);
            Position position = new Position(Position);

            properties.Add(healthLeft);
            properties.Add(position);

            return properties;
        }

        public override float EvaluateProperties(PropertyList properties)
        {
            Health health = properties.Get<Health>();

            float damageToTarget = 0;
            float evaluation = 0;

            if (properties.TryGet(out DamageToTarget damageProperty))
                damageToTarget = damageProperty.Evaluation;

            if (IsPlayer)
            {
                evaluation = (m_health / health.Value) * health.Weight - damageToTarget;
            }
            else
            {
                evaluation = health.Evaluation + damageToTarget;

                if (health.Value < 0)
                    evaluation += 100;

                if (properties.TryGet(out DistanceToTarget distanceProperty))
                    evaluation += (4 / distanceProperty.Value) * distanceProperty.Weight;
            }

            return evaluation;
        }
    }
}
