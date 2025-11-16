using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.StateMachines;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.UI
{
    public class GameUI : MonoBehaviour, ICombatStateListener
    {
        [SerializeField] private HealthBar m_healthBar;
        [SerializeField] private CombatUI m_combatUI;
        [SerializeField] private EnergyHUD m_energyHUD;
        [SerializeField] private RewardUI m_rewardUI;

        private List<ICombatStateListener> m_stateListeners;

        public HealthBar HealthBar => m_healthBar;
        public CombatUI CombatUI => m_combatUI;
        public EnergyHUD EnergyHUD => m_energyHUD;
        public RewardUI RewardUI => m_rewardUI;

        public void Initialize(Player player)
        {
            m_healthBar.Initialize(player);
            m_combatUI.Initialize(player);
            m_energyHUD.Initialize(player);

            m_stateListeners = new List<ICombatStateListener>()
            { m_energyHUD, m_combatUI };
        }

        public void EnterCombatState()
        {
            foreach (ICombatStateListener listener in m_stateListeners)
                listener.EnterCombatState();
        }

        public void ExitCombatState()
        {
            foreach (ICombatStateListener listener in m_stateListeners)
                listener.ExitCombatState();
        }
    }
}