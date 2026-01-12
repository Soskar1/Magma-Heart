using System.Collections.Generic;
using MagmaHeart.UIWindowPopupSystem.Definitions;
using UnityEngine;

namespace MagmaHeart.UIWindowPopupSystem
{
    public class WindowCreator
    {
        private readonly IDictionary<WindowTriggerDefinition, WindowData> m_database;
        private readonly WindowPresenter m_windowPrefab;
        private readonly Transform m_parent;

        public WindowCreator(WindowDatabaseDefinition databaseDefinition, WindowPresenter windowPrefab, Transform parentUI)
        {
            m_windowPrefab = windowPrefab;
            m_database = databaseDefinition.CreateDatabase();
            m_parent = parentUI;
        }

        public WindowPresenter Display(WindowTriggerDefinition trigger)
        {
            if (m_database.TryGetValue(trigger, out WindowData data))
            {
                WindowPresenter presenterInstance = GameObject.Instantiate(m_windowPrefab, m_parent);
                presenterInstance.Initialize(data);

                return presenterInstance;
            }

            return null;
        }
    }
}