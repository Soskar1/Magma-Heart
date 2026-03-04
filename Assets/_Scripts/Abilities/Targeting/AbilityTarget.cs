using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Abilities.Targeting
{
    public record AbilityTarget(TargetKind Kind, int EntityId, List<Vector3> Path)
    {
        public static AbilityTarget None = new AbilityTarget(TargetKind.None, 0, null);
        public static AbilityTarget EntityTarget(int entityId, List<Vector3> path) => new AbilityTarget(
                TargetKind.Entity | TargetKind.Path,
                entityId,
                path
            );
        public static AbilityTarget PathTarget(List<Vector3> path) => new AbilityTarget(TargetKind.Path, 0, path);
    }
}
