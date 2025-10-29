using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class TacticianAITests
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
                PossibleActions = new IAction[] {
                    new DamageAction(this, 4),
                    new MoveAction(this, 3),
                    new EngageAction(this, 4, 1)
                };
            }

            public override Dictionary<Type, PropertySnapshot> GetPropertySnapshots()
            {
                Dictionary<Type, PropertySnapshot> properties = base.GetPropertySnapshots();
                HealthLeft healthLeft = new HealthLeft(m_health);
                Position position = new Position(Position);

                properties.Add(typeof(HealthLeft), healthLeft);
                properties.Add(typeof(Position), position);

                return properties;
            }
        }

        internal record HealthLeft(float Value) : PropertySnapshot(Value, 1);
        internal record Position(Vector2 CurrentPosition) : PropertySnapshot(0, 0)
        {
            public float Distance(Position other) => Distance(other.CurrentPosition);
            public float Distance(Vector2 position) => Mathf.Abs(CurrentPosition.x - position.x) + Mathf.Abs(CurrentPosition.y - position.y);
        }
        internal record DamageToTarget(float Value) : PropertySnapshot(Value, 0.8f);
        internal record DistanceToTarget(float Value) : PropertySnapshot(Value, 0.5f);

        internal class DamageAction : IAction
        {
            public float Damage { get; init; }
            public AIUnit ActionPossessor { get; }

            public DamageAction(AIUnit actionPossessor, float damage)
            {
                ActionPossessor = actionPossessor;
                Damage = damage;
            }

            public void Execute() { }

            public bool CanSimulate(StateSnapshot state, AIUnit target)
            {
                Position possessorPosition = (Position)state.GetProperty(ActionPossessor, typeof(Position));
                Position targetPosition = (Position)state.GetProperty(target, typeof(Position));

                if (possessorPosition.Distance(targetPosition) > 1)
                    return false;

                return true;
            }

            public StateSnapshot Simulate(StateSnapshot state, AIUnit target)
            {
                List<PropertySnapshot> propertiesToAdd = new List<PropertySnapshot>();
                DamageToTarget damageToTarget = new DamageToTarget(Damage);
                propertiesToAdd.Add(damageToTarget);

                HealthLeft targetHealth = (HealthLeft)state.GetProperty(target, typeof(HealthLeft));
                IsAliveProperty isAliveProperty = null;

                if (targetHealth.Value < Damage)
                {
                    isAliveProperty = new IsAliveProperty(false);
                    propertiesToAdd.Add(new HealthLeft(-100));
                }
                else
                {
                    propertiesToAdd.Add(new HealthLeft(targetHealth.Value - Damage));
                }

                StateSnapshot newState = state with
                {
                    StateProperties = state.StateProperties
                };

                newState.Add(target, propertiesToAdd);
                
                if (isAliveProperty != null)
                    newState.Replace(target, isAliveProperty);

                return newState;
            }
        }

        internal class MoveAction : IAction
        {
            public AIUnit ActionPossessor { get; }
            public float m_speed;

            public MoveAction(AIUnit actionPossessor, float speed)
            {
                ActionPossessor = actionPossessor;
                m_speed = speed;
            }

            public void Execute() { }

            public bool CanSimulate(StateSnapshot state, AIUnit target)
            {
                Position possessorPosition = (Position)state.GetProperty(ActionPossessor, typeof(Position));
                Position targetPosition = (Position)state.GetProperty(target, typeof(Position));

                if (possessorPosition.Distance(targetPosition) <= 1)
                    return false;

                return true;
            }

            public StateSnapshot Simulate(StateSnapshot state, AIUnit target)
            {
                Position targetPosition = (Position)state.GetProperty(target, typeof(Position));
                Position possessorPosition = (Position)state.GetProperty(ActionPossessor, typeof(Position));

                Vector2 tmpPosition = possessorPosition.CurrentPosition;

                Vector2 direction = targetPosition.CurrentPosition - tmpPosition;
                float xMovement = Mathf.Min(Mathf.Abs(direction.x), m_speed);
                float yMovement = Mathf.Min(Mathf.Abs(direction.y), m_speed);

                if (direction.x > 0)
                    tmpPosition.x += xMovement;
                else if (direction.x < 0)
                    tmpPosition.x -= xMovement;

                if (direction.y > 0)
                    tmpPosition.y += yMovement;
                else if (direction.y < 0)
                    tmpPosition.y -= yMovement;


                StateSnapshot newState = state with
                {
                    StateProperties = state.StateProperties
                };

                DistanceToTarget distanceToTarget = new DistanceToTarget(4 / targetPosition.Distance(tmpPosition));
                newState.Replace(ActionPossessor, new List<PropertySnapshot> { distanceToTarget, new Position(tmpPosition) });

                return newState;
            }
        }

        internal class EngageAction : IAction
        {
            private MoveAction m_moveAction;
            private DamageAction m_damageAction;
            private float m_speed;

            public AIUnit ActionPossessor { get; }

            public EngageAction(AIUnit actionPossessor, float damage, float speed)
            {
                m_speed = speed;
                ActionPossessor = actionPossessor;
                m_moveAction = new MoveAction(actionPossessor, speed);
                m_damageAction = new DamageAction(actionPossessor, damage);
            }

            public void Execute() { }

            public bool CanSimulate(StateSnapshot state, AIUnit target)
            {
                Position possessorPosition = (Position)state.GetProperty(ActionPossessor, typeof(Position));
                Position targetPosition = (Position)state.GetProperty(target, typeof(Position));

                if (possessorPosition.Distance(targetPosition) > m_speed)
                    return false;

                return true;
            }

            public StateSnapshot Simulate(StateSnapshot state, AIUnit target)
            {
                StateSnapshot moveState = m_moveAction.Simulate(state, target);
                return m_damageAction.Simulate(moveState, target);
            }
        }



        public void SetUp()
        {

        }

        [Test]
        public void TacticianAI_ChooseBestMoveDepth1_ChoosesBestMoveBetweenMultipleActions()
        {
            Entity player = new Entity(10, new Vector2(5, 5), true);
            Func<StateSnapshot, AIUnit> enemySelection = (state) =>
            {
                AIUnit nearestUnit = null;
                float minDistance = float.MaxValue;

                List<AIUnit> allUnits = state.GetAllUnits();
                foreach (AIUnit unit in allUnits)
                {
                    if (unit.IsPlayer)
                        continue;

                    IsAliveProperty isAlive = (IsAliveProperty)state.GetProperty(unit, typeof(IsAliveProperty));
                    if (!isAlive)
                        continue;

                    Position unitPosition = (Position)state.GetProperty(unit, typeof(Position));

                    float distance = unitPosition.Distance(player.Position);
                    if (distance < minDistance)
                    {
                        nearestUnit = unit;
                        minDistance = distance;
                    }
                }

                return nearestUnit;
            };

            TacticianAI tactician = new TacticianAI(1, player, enemySelection);

            Entity enemy1 = new Entity(4, new Vector2(0, 0), false);
            Entity enemy2 = new Entity(4, new Vector2(2, 2), false);
            Entity enemy3 = new Entity(4, new Vector2(10, 10), false);

        }
    }
}
