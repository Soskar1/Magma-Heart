using System.Collections.Generic;
using MagmaHeart.UIWindowPopupSystem.Definitions;
using UnityEngine;

namespace MagmaHeart.UIWindowPopupSystem
{
    public class WindowDatabase
    {
        private Dictionary<WindowPopupTriggerDefinition, WindowData> m_database;

        public WindowDatabase(IEnumerable<WindowDefinition> definitions)
        {
            m_database = new Dictionary<WindowPopupTriggerDefinition, WindowData>();

            foreach (WindowDefinition definition in definitions)
                m_database.Add(definition.Trigger, definition.Data);
        }

        public WindowData GetData(WindowPopupTriggerDefinition definition)
        {
            if (m_database.TryGetValue(definition, out WindowData data))
                return data;


            Debug.LogError($"WindowDatabase does not contain {definition.name} trigger");
            return null;
        }
    }
}