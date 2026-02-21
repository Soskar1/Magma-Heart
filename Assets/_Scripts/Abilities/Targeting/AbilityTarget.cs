using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Abilities.Targeting
{
    [Serializable]
    public struct AbilityTarget
    {
        public TargetKind Kind { get; init; }
        public int EntityId { get; init; }
        public Vector3Int Position { get; init; }
        public List<Vector3Int> Path { get; init; }

        public static AbilityTarget None() => new() { Kind = TargetKind.None };
        public static AbilityTarget EntityTarget(int entityId) => new() { Kind = TargetKind.Entity, EntityId = entityId };
        public static AbilityTarget PositionTarget(Vector3Int position) => new() { Kind = TargetKind.Position, Position = position };
        public static AbilityTarget PathTarget(List<Vector3Int> path) => new() { Kind = TargetKind.Path, Path = path };
    }
}
