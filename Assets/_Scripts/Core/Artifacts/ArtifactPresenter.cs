using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.Artifacts
{
    public class ArtifactPresenter : MonoBehaviour
    {
        [SerializeField] private Artifact m_artifact;
        [SerializeField] private Image m_image;

        public void Initialize(Artifact artifact)
        {
            m_artifact = artifact;
            m_image.sprite = artifact.Data.Icon;
        }
    }
}