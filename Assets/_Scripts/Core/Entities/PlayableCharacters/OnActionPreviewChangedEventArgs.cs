using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.Dungeon;
using System;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class OnActionPreviewChangedEventArgs : EventArgs
    {
        public ActionPreview ActionPreview { get; init; }
        public RoomTile Tile { get; init; }
        public CombatBoardState State { get; init; }

        public OnActionPreviewChangedEventArgs(ActionPreview actionPreview, RoomTile tile, CombatBoardState state)
        {
            ActionPreview = actionPreview;
            Tile = tile;
            State = state;
        }
    }
}
