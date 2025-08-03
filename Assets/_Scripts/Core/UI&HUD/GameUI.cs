using System.Collections.Generic;
using MagmaHeart.Core.Entities.PlayableCharacters;
using UnityEngine;

namespace MagmaHeart.Core.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private HealthBar m_healthBar;
        [SerializeField] private CombatUI m_combatUI;
        [SerializeField] private EnergyHUD m_energyHUD;

        public List<IDisplayable> CombatRelatedUI { get; private set; } = new List<IDisplayable>();

        public HealthBar HealthBar => m_healthBar;
        public CombatUI CombatUI => m_combatUI;
        public EnergyHUD EnergyHUD => m_energyHUD;

        public void Initialize(Player player)
        {
            m_healthBar.Initialize(player);
            m_combatUI.Initialize(player);
            m_energyHUD.Initialize(player);

            CombatRelatedUI.Add(m_energyHUD);
            CombatRelatedUI.Add(m_combatUI);
        }
    }
}