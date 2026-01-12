using MagmaHeart.Core.TutorialSystem;
using UnityEngine;

namespace Magmaheart.Core.Tutorial
{
    public class Tutorial
    {
        private const string m_saveKey = "TutorialFlags";
        private TutorialFlags m_currentFlags;

        public Tutorial()
        {
            // TODO: Change this after save system implementation
            m_currentFlags = (TutorialFlags)PlayerPrefs.GetInt(m_saveKey, (int)TutorialFlags.None);
        }

        public void SetFlag(TutorialFlags flags)
        {
            m_currentFlags |= flags;

            // TODO: Change this after save system implementation
            PlayerPrefs.SetInt(m_saveKey, (int)m_currentFlags);
            PlayerPrefs.Save();
        }

        public bool IsSet(TutorialFlags flags) => (m_currentFlags & flags) == flags;
    }
}