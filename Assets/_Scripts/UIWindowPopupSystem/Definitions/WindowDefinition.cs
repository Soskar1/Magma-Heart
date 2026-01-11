using UnityEngine;

namespace MagmaHeart.UIWindowPopupSystem.Definitions
{
    public class WindowDefinition : ScriptableObject
    {
        [SerializeField] private WindowData m_data;
        [SerializeField] private WindowPopupTriggerDefinition m_trigger;

        public WindowData Data => m_data;
        public WindowPopupTriggerDefinition Trigger => m_trigger;
    }
}