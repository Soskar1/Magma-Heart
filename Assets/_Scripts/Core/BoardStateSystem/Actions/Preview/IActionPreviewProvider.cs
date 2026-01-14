using MagmaHeart.Core.BoardStateSystem.Actions.Preview;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.PlayableCharacters;
using System;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public interface IActionPreviewProvider
    {
        public ActionPreview Preview(RoomTile tile);

        public event EventHandler<OnActionPreviewChangedEventArgs> OnActionPreviewChanged;
    }
}
