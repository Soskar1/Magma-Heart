using MagmaHeart.Core.BoardStateSystem.Actions;
using System;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class OnActionPreviewChangedEventArgs : EventArgs
    {
        public ActionPreview ActionPreview { get; init; }
        public OnActionPreviewChangedEventArgs(ActionPreview actionPreview) => ActionPreview = actionPreview;
    }
}
