using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.Artifacts.Presentation
{
    public class AbilityPresenter : MonoBehaviour
    {
        [SerializeField] private Image m_image;
        private Artifact m_artifact;

        public void Initialize(Artifact artifact)
        {
            m_artifact = artifact;
            m_image.sprite = m_artifact.Data.Icon;
        }
    }
}