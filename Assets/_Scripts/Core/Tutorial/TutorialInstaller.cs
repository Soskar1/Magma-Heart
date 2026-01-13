using System.Collections.Generic;
using System.Linq;
using MagmaHeart.Core.SceneLoading;
using MagmaHeart.UIWindowPopupSystem;
using MagmaHeart.UIWindowPopupSystem.Definitions;
using UnityEngine;

namespace MagmaHeart.Core.TutorialSystem
{
    public class TutorialInstaller : IInstaller
    {
        private TutorialPresenter m_presenter;

        public TutorialContext Install(WindowDatabaseDefinition databaseDefinition, TutorialWindowPresenter windowPrefab, Transform parentUi)
        {
            IDictionary<WindowTriggerDefinition, WindowData> database = databaseDefinition.CreateDatabase();
            WindowCreator windowCreator = new WindowCreator(database, windowPrefab, parentUi);

            IEnumerable<TutorialTriggerDefinition> triggers = database.Keys
                .Where(trigger => trigger as TutorialTriggerDefinition != null)
                .Cast<TutorialTriggerDefinition>()
                .ToList();

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