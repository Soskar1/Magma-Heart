using System.Collections.Generic;
using MagmaHeart.Core.Artifacts;

namespace MagmaHeart.Core.SceneLoading
{
    public struct DataTransfer
    {
        public List<Artifact> ObtainedArtifacts { get; private set; }

        public void SaveObtainedArtifacts(List<Artifact> obtainedArtifacts) => ObtainedArtifacts = obtainedArtifacts;
    }
}