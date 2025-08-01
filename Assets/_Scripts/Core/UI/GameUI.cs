using MagmaHeart.Core.Entities.PlayableCharacters;
using UnityEngine;

namespace MagmaHeart.Core.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private HealthBar m_healthBar;
        [SerializeField] private CombatUI m_combatUI;
        public HealthBar HealthBar => m_healthBar;
        public CombatUI CombatUI => m_combatUI;

        public void Initialize(Player player)
        {
            m_healthBar.Initialize(player);
            m_combatUI.Initialize(player);
        }
    }
}