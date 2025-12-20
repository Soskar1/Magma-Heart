using MagmaHeart.AI.Actions;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    [CreateAssetMenu(fileName = "EntityData", menuName = "MagmaHeartData/EntityData")]
    public class EntityData : ScriptableObject
    {
        [SerializeField] private string m_name;
        [SerializeField] private Sprite m_portraitImage;
        [SerializeField] private EntityStats m_stats;
        [SerializeField] private List<ActionData> m_actions;

        public string Name => m_name;
        public Sprite PortraitImage => m_portraitImage;
        public EntityStats Stats => m_stats;
        public List<ActionData> Actions => m_actions;

        public EntityData(string name, EntityStats stats, List<ActionData> actions)
        {
            m_name = name;
            m_stats = stats;
            m_actions = actions;
        }
    }
}