using MagmaHeart.Abilities.Resources;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Abilities
{
    public interface IGameWorld
    {
        public IParameter GetParameter(int entityId, ParameterId parameter);
        public Vector3 GetEntityPosition(int entityId);
        public bool TryFindPath(Vector3 from, Vector3 to, out List<Vector3> path);
        public int GetEntityAtPosition(Vector3 position);
        public bool PositionIsAccessible(Vector3 position);
        public bool AreEnemiesToEachOther(int executorId, int targetId);
        public int GetDistance(int entityId1, int entityId2);
    }
}
