using System;
using UnityEngine;

namespace MagmaHeart.Core.TutorialSystem
{
    public class TutorialModel
    {
        private const string m_saveKey = "TutorialFlags";
        private TutorialFlags m_currentFlags;

        public event EventHandler<OnTutorialFlagSetEventArgs> OnTutorialFlagSet;

        public TutorialModel()
        {
            // TODO: Change this after save system implementation
            m_currentFlags = (TutorialFlags)PlayerPrefs.GetInt(m_saveKey, (int)TutorialFlags.None);
        }

        public void TrySetFlag(TutorialFlags flags)
        {
            if (IsSet(flags))
                return;

            m_currentFlags |= flags;

            OnTutorialFlagSetEventArgs args = new OnTutorialFlagSetEventArgs(flags);
            OnTutorialFlagSet?.Invoke(this, args);

            // TODO: Change this after save system implementation
            PlayerPrefs.SetInt(m_saveKey, (int)m_currentFlags);
            PlayerPrefs.Save();
        }

        public bool IsSet(TutorialFlags flags) => (m_currentFlags & flags) == flags;
    }
}