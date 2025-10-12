using MagmaHeart.Core.Artifacts;
using System;

namespace MagmaHeart.Core.UI
{
    public class OnRewardPickedArgs : EventArgs
    {
        public ArtifactData ArtifactData { get; init; }

        public OnRewardPickedArgs(ArtifactData artifactData) => ArtifactData = artifactData;
    }
}
