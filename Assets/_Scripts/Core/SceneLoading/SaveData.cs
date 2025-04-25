using System.Collections.Generic;
using MagmaHeart.Core.Artifacts;

namespace MagmaHeart.Core.SceneLoading
{
    public class SaveData
    {
        public float health;
        public List<Artifact> ObtainedArtifacts { get; private set; }

        public void SaveObtainedArtifacts(List<Artifact> obtainedArtifacts) => ObtainedArtifacts = obtainedArtifacts;
    }
}