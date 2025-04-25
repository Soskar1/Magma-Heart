using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Artifacts
{
    public class ArtifactApplier : MonoBehaviour
    {
        [SerializeField] private string m_attackSpeedParameter;
        public List<Artifact> ObtainedArtifacts { get; private set; }

        public Action<float> IncreaseHealth;
        public Action<float> IncreaseDamage;
        public Action<float> IncreaseSpeed;
        public Action<string, float> IncreaseAttackSpeed;

        private void Awake() => ObtainedArtifacts = new List<Artifact>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Artifact artifact))
            {
                ApplyArtifact(artifact);
                Destroy(artifact.gameObject);
            }
        }

        public void ApplyArtifacts(List<Artifact> artifacts)
        {
            foreach (Artifact artifact in artifacts)
                ApplyArtifact(artifact);
        }

        private void ApplyArtifact(Artifact artifact)
        {
            if (artifact is Apple appleArtifact)
                IncreaseHealth.Invoke(appleArtifact.HealthAmount);
            else if (artifact is StoneFist stoneFist)
                IncreaseDamage.Invoke(stoneFist.DamageAmount);
            else if (artifact is Boots boots)
                IncreaseSpeed.Invoke(boots.SpeedAmount);
            else if (artifact is EnergyDrink energyDrink)
                IncreaseAttackSpeed.Invoke(m_attackSpeedParameter, energyDrink.AttackSpeedAmount);

            ObtainedArtifacts.Add(artifact);
        }
    }
}