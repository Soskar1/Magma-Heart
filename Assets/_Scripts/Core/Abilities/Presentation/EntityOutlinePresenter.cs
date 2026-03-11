using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Presentation
{
    public enum OutlineType
    {
        None,
        Ally,
        Enemy,
        CanBeAttacked
    }

    public class EntityOutlinePresenter : MonoBehaviour
    {
        [SerializeField] private Material m_allyOutline;
        [SerializeField] private Material m_enemyOutline;
        [SerializeField] private Material m_canAttackOutline;

        public void OutlineEntity(Entity entity, OutlineType outlineType)
        {
            switch (outlineType)
            {
                case OutlineType.None:
                    break;
                case OutlineType.Enemy:
                    entity.Outline.ApplyOutline(m_enemyOutline);
                    break;
                case OutlineType.Ally:
                    entity.Outline.ApplyOutline(m_allyOutline);
                    break;
                case OutlineType.CanBeAttacked:
                    entity.Outline.ApplyOutline(m_canAttackOutline);
                    break;
                default:
                    Debug.LogWarning($"Outline type '{outlineType}' is not implemented");
                    break;
            }
        }

        public void ClearOutline(Entity entity) => entity.Outline.RemoveOutline();
    }
}
