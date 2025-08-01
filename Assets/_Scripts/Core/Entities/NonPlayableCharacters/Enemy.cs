using UnityEngine;

namespace MagmaHeart.Core.Entities.NonPlayableCharacters
{
    public class Enemy : MonoBehaviour, IEntity
    {
        [SerializeField] private float m_maxHealth;

        private Entity m_controllingEntity;
        public Entity ControllingEntity => m_controllingEntity;

        public void Initialize()
        {
            m_controllingEntity = new Entity(m_maxHealth);
        }
    }
}