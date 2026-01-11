using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.UIWindowPopupSystem.Definitions
{
    public class WindowDatabaseDefinition : ScriptableObject
    {
        [SerializeField] private List<WindowDefinition> m_windows = new List<WindowDefinition>();

        public WindowDatabase CreateDatabase() => new WindowDatabase(m_windows);
    }
}