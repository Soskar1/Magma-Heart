using MagmaHeart.AI.States;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record Health(float CurrentHealth, float MaxHealth) : PropertySnapshot() { }

    internal record Position(Vector2 CurrentPosition) : PropertySnapshot()
    {
        public float Distance(Position other) => Distance(other.CurrentPosition);
        public float Distance(Vector2 position) => Mathf.Abs(CurrentPosition.x - position.x) + Mathf.Abs(CurrentPosition.y - position.y);

        public static implicit operator Vector2(Position property) => property.CurrentPosition;
    }
}
