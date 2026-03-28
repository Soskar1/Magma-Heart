using UnityEngine;

namespace MagmaHeart.Core.Entities.Presenters
{
    public class VFXPresenter : MonoBehaviour
    {
        [SerializeField] private ParticleSystem m_vfx;

        public void Play()
        {
            m_vfx.Play();
        }
    }
}
