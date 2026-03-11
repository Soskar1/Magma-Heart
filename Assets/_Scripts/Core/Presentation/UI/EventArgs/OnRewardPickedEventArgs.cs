using MagmaHeart.Core.Artifacts;
using System;

namespace MagmaHeart.Core.Presentation.UI
{
    public class OnRewardPickedEventArgs : EventArgs
    {
        public ArtifactData ArtifactData { get; init; }

        public OnRewardPickedEventArgs(ArtifactData artifactData) => ArtifactData = artifactData;
    }
}
