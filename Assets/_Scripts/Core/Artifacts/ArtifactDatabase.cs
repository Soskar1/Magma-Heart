using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Artifacts
{
    [CreateAssetMenu(menuName = "Magma Heart Data/Artifacts/Artifact Database")]
    public class ArtifactDatabase : ScriptableObject
    {
        [SerializeField] private List<ArtifactData> m_artifacts;

        public List<ArtifactData> Artifacts => m_artifacts;
    }
}
