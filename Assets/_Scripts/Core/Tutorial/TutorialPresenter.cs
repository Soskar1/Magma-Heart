using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MagmaHeart.UIWindowPopupSystem;

namespace MagmaHeart.Core.TutorialSystem
{
    public class TutorialPresenter : IDisposable
    {
        private readonly TutorialModel m_model;
        private readonly WindowCreator m_windowCreator;
        private readonly Dictionary<TutorialFlags, TutorialTriggerDefinition> m_triggers;
        private readonly Dictionary<TutorialFlags, TutorialWindowPresenter> m_windows;
        
        public TutorialPresenter(TutorialModel model, IEnumerable<TutorialTriggerDefinition> tutorialTriggers, WindowCreator windowCreator)
        {
            m_model = model;
            m_windowCreator = windowCreator;
            m_triggers = new Dictionary<TutorialFlags, TutorialTriggerDefinition>();
            m_windows = new Dictionary<TutorialFlags, TutorialWindowPresenter>();

            foreach (TutorialTriggerDefinition trigger in tutorialTriggers)
                m_triggers.Add(trigger.Flag, trigger);

            m_model.OnTutorialFlagSet += HandleOnTutorialFlagSet;
        }

        public void Dispose()
        {
            m_model.OnTutorialFlagSet -= HandleOnTutorialFlagSet;
        }

        public Task GetUntilWindowCloseTask(TutorialFlags flag)
        {
            if (m_windows.TryGetValue(flag, out TutorialWindowPresenter tutorialWindow))
                return tutorialWindow.GetTask();

            return Task.CompletedTask;
        }

        private void HandleOnTutorialFlagSet(object _, OnTutorialFlagSetEventArgs args)
        {
            if (m_triggers.TryGetValue(args.NewFlag, out TutorialTriggerDefinition trigger))
            {
                TutorialWindowPresenter tutorialWindow = (TutorialWindowPresenter)m_windowCreator.Display(trigger);
                m_windows.Add(args.NewFlag, tutorialWindow);
            }
        }
    }
}