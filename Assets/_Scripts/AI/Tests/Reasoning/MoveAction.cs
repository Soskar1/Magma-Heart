using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
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
            Position targetPosition = state.GetProperty<Position>(target);
            Position possessorPosition = state.GetProperty<Position>(ActionPossessor);

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
                StateProperties = state.StateProperties.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.ToDictionary(
                        inner => inner.Key,
                        inner => inner.Value
                    )
                )
            };

            float pointsForDistance = Math.Max(targetPosition.Distance(tmpPosition), 0.5f);
            DistanceToTarget distanceToTarget = new DistanceToTarget(4 / pointsForDistance);
            newState.Replace(ActionPossessor, new List<PropertySnapshot> { distanceToTarget, new Position(tmpPosition) });

            return newState;
        }
    }
}
