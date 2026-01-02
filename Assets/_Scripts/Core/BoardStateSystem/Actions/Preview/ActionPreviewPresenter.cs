using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Presentation;

namespace MagmaHeart.Core.BoardStateSystem.Actions.Preview
{
    public class CombatActionPreviewPresenter : IActionPreviewPresenter
    {
        private readonly IActionPreviewProvider m_previewProvider;
        private readonly ICombatTileHighlighter m_tileHighlighter;
        private readonly DungeonController m_dungeonController;

        public CombatActionPreviewPresenter(DungeonController dungeonController, IActionPreviewProvider provider, ICombatTileHighlighter tileHighlighter)
        {
            m_previewProvider = provider;
            m_tileHighlighter = tileHighlighter;
            m_dungeonController = dungeonController;

            m_previewProvider.OnActionPreviewChanged += HandleOnActionPreviewChanged;
        }

        public void Disable()
        {
            m_previewProvider.OnActionPreviewChanged -= HandleOnActionPreviewChanged;
        }

        public void Present(ActionPreview preview)
        {
            m_tileHighlighter.Clear();
            
            if (preview == null)
                return;

            if (preview.Action is MovementAction)
            {

                MovementActionArgs args = (MovementActionArgs)preview.Args;
                RoomTile tile = m_dungeonController.CurrentRoom.GetRoomTile(args.TargetTile);

                m_tileHighlighter.Show(tile);
            }
            else if (preview.Action is AttackAction)
            {
                AttackActionArgs args = (AttackActionArgs)preview.Args;
                m_dungeonController.CurrentRoom.TryGetEntity(args.Target, out Entity entity);
                entity.Outline.ApplyOutline(OutlineSettings.CAN_ATTACK_OUTLINE);
            }
        }

        private void HandleOnActionPreviewChanged(object obj, OnActionPreviewChangedEventArgs args) => Present(args.ActionPreview);
    }
}
