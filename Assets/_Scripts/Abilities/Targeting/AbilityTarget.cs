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
        public Vector3 Position { get; init; }
        public List<Vector3> Path { get; init; }

        public static AbilityTarget None() => new() { Kind = TargetKind.None };
        public static AbilityTarget EntityTarget(int entityId, List<Vector3> path) => new() { 
                Kind = TargetKind.Entity | TargetKind.Path,
                EntityId = entityId,
                Path = path
            };
        public static AbilityTarget PositionTarget(Vector3 position) => new() { Kind = TargetKind.Position, Position = position };
        public static AbilityTarget PathTarget(List<Vector3> path) => new() { Kind = TargetKind.Path, Path = path };
    }
}
