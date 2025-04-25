using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.SceneLoading
{
    public class Teleporter : MonoBehaviour
    {
        [SerializeField] private SceneLoader m_loader;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<PlayerBehaviour>() != null)
            {
                ArtifactApplier applier = collision.GetComponent<ArtifactApplier>();

                DataTransfer dataTransfer = new DataTransfer();
                dataTransfer.SaveObtainedArtifacts(applier.ObtainedArtifacts);
                m_loader.LoadNextScene(dataTransfer);
            }
        }
    }
}