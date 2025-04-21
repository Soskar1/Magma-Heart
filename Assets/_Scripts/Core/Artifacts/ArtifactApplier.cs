using System;
using UnityEngine;

namespace MagmaHeart.Core.Artifacts
{
    public class ArtifactApplier : MonoBehaviour
    {
        public Action<float> IncreaseHealth;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Artifact artifact))
            {
                if (artifact is Apple appleArtifact)
                    IncreaseHealth.Invoke(appleArtifact.HealthAmount);

                Destroy(artifact.gameObject);
            }
        }
    }
}