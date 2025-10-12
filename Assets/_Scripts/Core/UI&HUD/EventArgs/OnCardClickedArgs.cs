using MagmaHeart.Core.Artifacts;
using System;

namespace MagmaHeart.Core.UI
{
    public class OnCardClickedArgs : EventArgs
    {
        public ArtifactData ClickedArtifactData { get; init; }

        public OnCardClickedArgs(ArtifactData clickedArtifactData) => ClickedArtifactData = clickedArtifactData;
    }
}
