using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Entities.Presenters
{
    public class EntityEffectsPresenter : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> m_effects;

        public void Show()
        {
            foreach (var effect in m_effects)
                effect.Play();
        }

        public void Hide()
        {
            foreach (var effect in m_effects)
                effect.Stop();
        }
    }
}
