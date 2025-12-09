using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    [CreateAssetMenu(fileName = "EntityData", menuName = "MagmaHeartData/EntityData")]
    public class EntityData : ScriptableObject
    {
        [SerializeField] private string m_name;
        [SerializeField] private Sprite m_portraitImage;
        [SerializeField] private EntityStats m_stats;

        public string Name => m_name;
        public Sprite PortraitImage => m_portraitImage;
        public EntityStats Stats => m_stats;

        public EntityData(string name, EntityStats stats)
        {
            m_name = name;
            m_stats = stats;
        }
    }
}