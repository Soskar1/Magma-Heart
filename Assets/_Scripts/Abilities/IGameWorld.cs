using MagmaHeart.Abilities.Resources;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Abilities
{
    public interface IGameWorld
    {
        public int GetResource(int entityId, ResourceId resource);
        public Vector2Int GetPosition(int entityId);
        public List<Vector2Int> FindPath(Vector2Int from, Vector2Int to);
        public bool IsEnemy(int executorId, int targetId);
        public int GetDistance(int entityId1, int entityId2);
    }
}
