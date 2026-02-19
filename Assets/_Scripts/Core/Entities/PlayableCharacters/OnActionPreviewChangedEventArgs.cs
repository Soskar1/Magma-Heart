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
        public Room Room { get; init; }

        public OnActionPreviewChangedEventArgs(ActionPreview actionPreview, RoomTile tile, Room state)
        {
            ActionPreview = actionPreview;
            Tile = tile;
            Room = state;
        }
    }
}
