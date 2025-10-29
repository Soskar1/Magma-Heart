using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record Health(float Value) : PropertySnapshot(Value, 1)
    {
        public static implicit operator float(Health property) => property.Value;
    }

    internal record Position(Vector2 CurrentPosition) : PropertySnapshot(0, 0)
    {
        public float Distance(Position other) => Distance(other.CurrentPosition);
        public float Distance(Vector2 position) => Mathf.Abs(CurrentPosition.x - position.x) + Mathf.Abs(CurrentPosition.y - position.y);

        public static implicit operator Vector2(Position property) => property.CurrentPosition;
    }
    internal record DamageToTarget(float Value) : PropertySnapshot(Value, 0.8f);
    internal record DistanceToTarget(float Value) : PropertySnapshot(Value, 0.5f);
}
