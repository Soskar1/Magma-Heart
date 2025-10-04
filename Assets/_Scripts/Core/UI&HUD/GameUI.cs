using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Entities.PlayableCharacters;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.UI
{
    public class GameUI : MonoBehaviour, ICombatStateListener, ICombatTurnSwitchListener
    {
        [SerializeField] private HealthBar m_healthBar;
        [SerializeField] private CombatUI m_combatUI;
        [SerializeField] private EnergyHUD m_energyHUD;

        private List<ICombatTurnSwitchListener> m_turnSwitchListeners;
        private List<ICombatStateListener> m_stateListeners;

        public HealthBar HealthBar => m_healthBar;
        public CombatUI CombatUI => m_combatUI;
        public EnergyHUD EnergyHUD => m_energyHUD;

        public void Initialize(Player player)
        {
            m_healthBar.Initialize(player);
            m_combatUI.Initialize(player);
            m_energyHUD.Initialize(player);

            m_turnSwitchListeners = new List<ICombatTurnSwitchListener>()
            { m_energyHUD, m_combatUI };
            m_stateListeners = new List<ICombatStateListener>()
            { m_energyHUD, m_combatUI };
        }

        public void HandleOnTurnSwitched(object obj, OnTurnSwitchedEventArgs args)
        {
            foreach (ICombatTurnSwitchListener listener in m_turnSwitchListeners)
                listener.HandleOnTurnSwitched(obj, args);
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