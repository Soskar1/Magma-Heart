using MagmaHeart.Abilities.Resources;
using UnityEngine;

namespace MagmaHeart.Abilities
{
    public interface IGameWorld
    {
        public int GetResource(int entityId, ResourceId resource);
        public Vector2Int GetPosition(int entityId);
        public bool IsEnemy(int executorId, int targetId);
    }
}
