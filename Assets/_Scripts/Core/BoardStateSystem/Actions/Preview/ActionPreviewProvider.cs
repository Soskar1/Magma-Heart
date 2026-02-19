using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.PlayableCharacters;
using System;

namespace MagmaHeart.Core.BoardStateSystem.Actions.Preview
{
    public class ActionPreviewProvider : IActionPreviewProvider
    {
        private readonly IActionPreviewService m_actionPreviewService;
        private readonly Battle m_battle;
        private EntityModel m_executor;
        private Room m_currentRoom;
        private ActionPreview m_currentPreview;

        public event EventHandler<OnActionPreviewChangedEventArgs> OnActionPreviewChanged;

        public ActionPreviewProvider(IActionPreviewService actionPreviewService, Battle battle)
        {
            m_actionPreviewService = actionPreviewService;
            m_battle = battle;

            m_battle.OnBattleStarted += HandleOnBattleStarted;
            m_battle.OnTurnSwitched += HandleOnTurnSwitched;
        }

        public void Disable()
        {
            m_battle.OnBattleStarted -= HandleOnBattleStarted;
            m_battle.OnTurnSwitched -= HandleOnTurnSwitched;
        }

        private void HandleOnBattleStarted(object obj, OnBattleStartedEventArgs args) => m_currentRoom = args.Room;
        private void HandleOnTurnSwitched(object obj, OnTurnSwitchedEventArgs args)
        {
            if (args.CurrentEntity.Model.IsPlayer)
                m_executor = args.CurrentEntity.Model;
            else
                m_executor = null;
        }

        public ActionPreview Preview(RoomTile tile)
        {
            ActionPreview newPreview = null;

            if (tile != null)
                newPreview = m_actionPreviewService.Preview(m_currentRoom, m_executor, tile);

            m_currentPreview = newPreview;

            OnActionPreviewChangedEventArgs args = new OnActionPreviewChangedEventArgs(newPreview, tile, m_currentRoom);
            OnActionPreviewChanged?.Invoke(this, args);

            return m_currentPreview;
        }
    }
}
