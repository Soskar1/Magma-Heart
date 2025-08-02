using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    [CreateAssetMenu(fileName = "EntityData", menuName = "MagmaHeartData/EntityData")]
    public class EntityData : ScriptableObject
    {
        [SerializeField] private string m_name;
        [SerializeField] private EntityStats m_stats;

        public string Name => m_name;
        public EntityStats Stats => m_stats;
    }
}