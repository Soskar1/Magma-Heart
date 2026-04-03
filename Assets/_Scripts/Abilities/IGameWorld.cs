using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Abilities
{
    public interface IGameWorld
    {
        public IParameter GetParameter(int entityId, ParameterId parameter);
        public void SetParameter(int entityId, ParameterId parameter, float newValue);
        public Vector3 GetEntityPosition(int entityId);
        public bool TryFindPath(Vector3 from, Vector3 to, out List<Vector3> path, bool ignoreEntities = false);
        public bool AreEnemiesToEachOther(int executorId, int targetId);
        public int GetDistance(int entityId1, int entityId2);
        public int GetCooldown(int entityId, string abilityId);
        public IReadOnlyList<int> GetAllEntities();
    }
}
