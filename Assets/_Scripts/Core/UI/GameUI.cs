using UnityEngine;

namespace MagmaHeart.Core.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private HealthBar m_healthBar;
        public HealthBar HealthBar => m_healthBar;
    }
}