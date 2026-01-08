using System;

namespace MagmaHeart.Core.Artifacts
{
    public class OnArtifactLeveledUpEventArgs : EventArgs
    {
        public Artifact Artifact { get; init; }

        public OnArtifactLeveledUpEventArgs(Artifact artifact) => Artifact = artifact;
    }
}
