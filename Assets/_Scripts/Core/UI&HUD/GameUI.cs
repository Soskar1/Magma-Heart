using System.Collections.Generic;
using MagmaHeart.Core.Entities.PlayableCharacters;
using UnityEngine;

namespace MagmaHeart.Core.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private HealthBar m_healthBar;
        [SerializeField] private CombatUI m_combatUI;
        [SerializeField] private CombatHUD m_combatHUD;

        public List<IDisplayable> CombatRelatedUI { get; private set; } = new List<IDisplayable>();

        public HealthBar HealthBar => m_healthBar;
        public CombatUI CombatUI => m_combatUI;
        public CombatHUD CombatHUD => m_combatHUD;

        public void Initialize(Player player)
        {
            m_healthBar.Initialize(player);
            m_combatUI.Initialize(player);
            m_combatHUD.Initialize(player);

            CombatRelatedUI.Add(m_combatHUD);
            CombatRelatedUI.Add(m_combatUI);
        }
    }
}