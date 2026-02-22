using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Abilities
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
        [SerializeField] private Color m_allyOutlineColor = Color.green;
        [SerializeField] private Color m_enemyOutlineColor = Color.red;
        [SerializeField] private Color m_canAttackOutlineColor = new Color(1, 0.35f, 0.35f);

        public void OutlineEntity(Entity entity, OutlineType outlineType)
        {
            switch (outlineType)
            {
                case OutlineType.None:
                    break;
                case OutlineType.Enemy:
                    entity.Outline.ApplyOutline(m_enemyOutlineColor);
                    break;
                case OutlineType.Ally:
                    entity.Outline.ApplyOutline(m_allyOutlineColor);
                    break;
                case OutlineType.CanBeAttacked:
                    entity.Outline.ApplyOutline(m_canAttackOutlineColor);
                    break;
                default:
                    Debug.LogWarning($"Outline type '{outlineType}' is not implemented");
                    break;
            }
        }

        public void ClearOutline(Entity entity) => entity.Outline.RemoveOutline();
    }
}
