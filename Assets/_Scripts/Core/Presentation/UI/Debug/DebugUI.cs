using UnityEngine;

namespace MagmaHeart.Core.Presentation.UI
{
    public class DebugUI : MonoBehaviour
    {
        [SerializeField] private SeedDisplay m_seedDisplay;

        public void Initialize(int seed)
        {
            m_seedDisplay.Initialize(seed);
        }
    }
}