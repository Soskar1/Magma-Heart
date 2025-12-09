using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.Entities.Presenters
{
    public class EntityPresenter : MonoBehaviour
    {
        [SerializeField] private HealthPresenter m_healthPresenter;
        [SerializeField] private Image m_portrait;
        private EntityModel m_entity;

        public void Initialize(EntityModel entity)
        {
            m_entity = entity;
            m_healthPresenter.Register(entity.Health);
            
            if (entity.Data.PortraitImage != null)
                m_portrait.sprite = entity.Data.PortraitImage;
        }

        // TODO: mouse hover must trigger entity outline and entity ui info popup
    }
}
