using System;
using UnityEngine;

namespace MagmaHeart.Core.Artifacts
{
    public class ArtifactApplier : MonoBehaviour
    {
        public Action<float> IncreaseHealth;
        public Action<float> IncreaseDamage;
        public Action<float> IncreaseSpeed;
        public Action<float> IncreaseAttackSpeed;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Artifact artifact))
            {
                if (artifact is Apple appleArtifact)
                    IncreaseHealth.Invoke(appleArtifact.HealthAmount);
                else if (artifact is StoneFist stoneFist)
                    IncreaseDamage.Invoke(stoneFist.DamageAmount);
                else if (artifact is Boots boots)
                    IncreaseSpeed.Invoke(boots.SpeedAmount);
                else if (artifact is EnergyDrink energyDrink)
                    IncreaseAttackSpeed.Invoke(energyDrink.AttackSpeedAmount);
                
                Destroy(artifact.gameObject);
            }
        }
    }
}