using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.UIWindowPopupSystem.Definitions
{
    public class WindowDatabaseDefinition : ScriptableObject
    {
        [SerializeField] private List<WindowDefinition> m_windows = new List<WindowDefinition>();

        public IDictionary<WindowTriggerDefinition, WindowData> CreateDatabase()
        {
            Dictionary<WindowTriggerDefinition, WindowData> database = new Dictionary<WindowTriggerDefinition, WindowData>();

            foreach (WindowDefinition window in m_windows)
            {
                if (window.Trigger == null)
                {
                    Debug.LogError($"WindowDefinition {window.name} has no trigger assigned.");
                    continue;
                }

                if (database.ContainsKey(window.Trigger))
                {
                    Debug.LogWarning($"Duplicate trigger {window.Trigger.name} found in database while trying to register {window.name} window");
                    continue;
                }

                
                if (string.IsNullOrEmpty(window.Data.Header) || string.IsNullOrEmpty(window.Data.Content))
                    Debug.LogWarning($"WindowDefinition {window.name} has incomplete data.");

                database.Add(window.Trigger, window.Data);
            }

            return database;
        }
    }
}