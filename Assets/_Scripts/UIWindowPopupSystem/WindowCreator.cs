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

        public WindowCreator(IDictionary<WindowTriggerDefinition, WindowData> database, WindowPresenter windowPrefab, Transform parentUI)
        {
            m_windowPrefab = windowPrefab;
            m_database = database;
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