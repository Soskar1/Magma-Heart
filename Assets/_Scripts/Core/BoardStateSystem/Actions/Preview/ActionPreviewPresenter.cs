using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Presentation;
using MagmaHeart.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions.Preview
{
    public class CombatActionPreviewPresenter : IActionPreviewPresenter
    {
        private readonly IActionPreviewProvider m_previewProvider;
        private readonly ICombatTileHighlighter m_tileHighlighter;
        private readonly GameWorld m_gameWorld;

        public CombatActionPreviewPresenter(GameWorld gameWorld, IActionPreviewProvider provider, ICombatTileHighlighter tileHighlighter)
        {
            m_previewProvider = provider;
            m_tileHighlighter = tileHighlighter;
            m_gameWorld = gameWorld;

            m_previewProvider.OnActionPreviewChanged += HandleOnActionPreviewChanged;
        }

        public void Disable()
        {
            m_previewProvider.OnActionPreviewChanged -= HandleOnActionPreviewChanged;
        }

        public void Present(ActionPreview preview, RoomTile tile, Room room)
        {
            m_tileHighlighter.Clear();
            
            if (preview == null)
                return;

            if (preview.Action is MovementAction movementAction)
            {
                MovementActionArgs args = (MovementActionArgs)preview.Args;

                List<Vector2> path = movementAction.CreatePath(args, room).Skip(1).ToList();

                bool isReachable = path.LastOrDefault().ToVector3Int() == tile.Position;
                m_tileHighlighter.Show(tile, isReachable);

                foreach (Vector2 point in path)
                {
                    RoomTile pathTile = m_gameWorld.GetTile(point);
                    m_tileHighlighter.Show(pathTile, true);
                }
            }
            else if (preview.Action is AttackAction)
            {
                AttackActionArgs args = (AttackActionArgs)preview.Args;
                m_gameWorld.TryGetEntity(args.TargetEntityInput.Target.Id, out Entity entity);
                entity.Outline.ApplyOutline(OutlineSettings.CAN_ATTACK_OUTLINE);
            }
        }

        private void HandleOnActionPreviewChanged(object obj, OnActionPreviewChangedEventArgs args) => Present(args.ActionPreview, args.Tile, args.Room);
    }
}
