using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.Input;
using System;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class PlayerCombatController : IActionPreviewProvider
    {
        private readonly IActionPreviewService m_actionPreviewService;
        private readonly PlayerTurnContext m_turnContext;
        private readonly MouseListener m_mouseListener;

        private ActionPreview m_currentPreview;

        public event Action<ActionPreview> OnPreviewChanged;

        public PlayerCombatController(PlayerTurnContext turnContext, MouseListener mouseListener, IActionPreviewService previewService)
        {
            m_turnContext = turnContext;
            m_mouseListener = mouseListener;
            m_actionPreviewService = previewService;
        }

        public void Enable() => m_mouseListener.OnGameLeftMouseButtonClick += HandleOnGameLeftMouseButtonClick;
        public void Disable() => m_mouseListener.OnGameLeftMouseButtonClick -= HandleOnGameLeftMouseButtonClick;

        public ActionPreview Preview(RoomTile tile)
        {
            ActionPreview newPreview = m_actionPreviewService.Preview(m_turnContext.CurrentCombatBoardState, m_turnContext.TypedModel, tile);

            if (Equals(m_currentPreview, newPreview))
                return m_currentPreview;

            m_currentPreview = newPreview;
            OnPreviewChanged?.Invoke(newPreview);

            return m_currentPreview;
        }

        private async void HandleOnGameLeftMouseButtonClick()
        {
            if (m_currentPreview == null)
                return;

            await m_turnContext.Execute(m_currentPreview);
        }
    }
}
