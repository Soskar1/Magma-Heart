using UnityEngine;

namespace MagmaHeart.UIWindowPopupSystem.Definitions
{
    public class WindowDefinition : ScriptableObject
    {
        [SerializeField] private WindowData m_data;
        [SerializeField] private WindowTriggerDefinition m_trigger;

        public WindowData Data => m_data;
        public WindowTriggerDefinition Trigger => m_trigger;
    }
}