using MagmaHeart.Abilities.Resources;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Abilities
{
    public interface IGameWorld
    {
        public float GetResource(int entityId, ResourceId resource);
        public Vector3Int GetPosition(int entityId);
        public List<Vector3Int> FindPath(Vector3Int from, Vector3Int to);
        public bool IsEnemy(int executorId, int targetId);
        public int GetDistance(int entityId1, int entityId2);
    }
}
