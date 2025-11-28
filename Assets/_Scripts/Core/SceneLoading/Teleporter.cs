using MagmaHeart.Core.Entities.PlayableCharacters;
using UnityEngine;

namespace MagmaHeart.Core.SceneLoading
{
    public class Teleporter : MonoBehaviour
    {
        private SceneLoader m_loader;

        public void Initialize(SceneLoader sceneLoader) => m_loader = sceneLoader;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player))
            {
                SaveData dataTransfer = new SaveData();
                // dataTransfer.SaveObtainedArtifacts(player.ArtifactApplier.ObtainedArtifacts);
                dataTransfer.health = player.Health.CurrentHealth;
                m_loader.LoadNextScene(dataTransfer);
            }
        }
    }
}