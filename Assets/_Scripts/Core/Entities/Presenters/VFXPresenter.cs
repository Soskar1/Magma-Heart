using UnityEngine;

namespace MagmaHeart.Core.Entities.Presenters
{
    public class VFXPresenter : MonoBehaviour
    {
        [SerializeField] private ParticleSystem m_stunAttackVfx;
        [SerializeField] private ParticleSystem m_chargeVfx;

        public void PlayStunAttackVfx()
        {
            m_stunAttackVfx.Play();
        }

        public void PlayChargeVfx()
        {
            m_chargeVfx.Play();
        }
    }
}
