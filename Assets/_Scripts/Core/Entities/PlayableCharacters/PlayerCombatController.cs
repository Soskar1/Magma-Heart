using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.Input;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class PlayerCombatController
    {
        private readonly IActionPreviewProvider m_actionPreviewProvider;
        private readonly PlayerTurnContext m_turnContext;
        private readonly MouseListener m_mouseListener;

        private ActionPreview m_currentPreview;

        public PlayerCombatController(PlayerTurnContext turnContext, MouseListener mouseListener, IActionPreviewProvider previewProvider)
        {
            m_turnContext = turnContext;
            m_mouseListener = mouseListener;
            m_actionPreviewProvider = previewProvider;
        }

        public void Enable()
        {
            m_mouseListener.OnGameLeftMouseButtonClick += HandleOnGameLeftMouseButtonClick;
            m_actionPreviewProvider.OnActionPreviewChanged += HandleOnActionPreviewChanged;
        }

        public void Disable()
        {
            m_mouseListener.OnGameLeftMouseButtonClick -= HandleOnGameLeftMouseButtonClick;
            m_actionPreviewProvider.OnActionPreviewChanged -= HandleOnActionPreviewChanged;
        }

        private async void HandleOnGameLeftMouseButtonClick()
        {
            if (m_currentPreview == null)
                return;

            await m_turnContext.Execute(m_currentPreview);
        }

        private void HandleOnActionPreviewChanged(object obj, OnActionPreviewChangedEventArgs args) => m_currentPreview = args.ActionPreview;
    }
}
