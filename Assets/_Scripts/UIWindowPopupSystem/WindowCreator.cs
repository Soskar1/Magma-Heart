using System;
using UnityEngine;

namespace MagmaHeart.UIWindowPopupSystem
{
    public class WindowCreator
    {
        private readonly WindowDatabase m_database;
        private readonly WindowPresenter m_windowPrefab;

        public WindowCreator(WindowDatabase database, WindowPresenter windowPrefab)
        {
            m_windowPrefab = windowPrefab;
            m_database = database;
        }

        public void Register(EventHandler<OnWindowTriggerEventArgs> trigger) => trigger += HandleOnWindowTrigger;
        public void Unregister(EventHandler<OnWindowTriggerEventArgs> trigger) => trigger -= HandleOnWindowTrigger;

        private void HandleOnWindowTrigger(object _, OnWindowTriggerEventArgs args)
        {
            WindowData data = m_database.GetData(args.Trigger);
            WindowPresenter presenterInstance = GameObject.Instantiate(m_windowPrefab);
            presenterInstance.Initialize(data);
        }
    }
}