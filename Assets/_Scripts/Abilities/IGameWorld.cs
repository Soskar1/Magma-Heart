using MagmaHeart.Abilities.Resources;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Abilities
{
    public interface IGameWorld
    {
        public float GetResource(int entityId, ResourceId resource);
        public Vector3 GetPosition(int entityId);
        public List<Vector3> FindPath(Vector3 from, Vector3 to);
        public int GetEntityAtPosition(Vector3 position);
        public bool PositionIsAccessible(Vector3 position);
        public bool IsEnemy(int executorId, int targetId);
        public int GetDistance(int entityId1, int entityId2);
    }
}
