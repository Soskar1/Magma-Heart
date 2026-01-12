using System.Collections.Generic;
using MagmaHeart.Core.SceneLoading;
using MagmaHeart.UIWindowPopupSystem;
using MagmaHeart.UIWindowPopupSystem.Definitions;
using UnityEngine;

namespace MagmaHeart.Core.TutorialSystem
{
    public class TutorialInstaller : IInstaller
    {
        private TutorialPresenter m_presenter;

        public TutorialContext Install(IEnumerable<TutorialTriggerDefinition> triggers, WindowDatabaseDefinition database, TutorialWindowPresenter windowPrefab, Transform parentUi)
        {
            WindowCreator windowCreator = new WindowCreator(database, windowPrefab, parentUi);

            TutorialModel model = new TutorialModel();
            m_presenter = new TutorialPresenter(model, triggers, windowCreator);

            return new TutorialContext(model, m_presenter);
        }

        public void Dispose()
        {
            m_presenter.Dispose();
        }
    }
}