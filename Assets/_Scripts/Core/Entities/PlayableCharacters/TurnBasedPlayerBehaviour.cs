using System;
using System.Collections.Generic;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.UI;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class TurnBasedPlayerBehaviour : IPlayerBehaviour, ITurnController
    {
        private UserInput m_userInput;
        private List<IDisplayable> m_combatUI;
        private Energy m_energy;

        public Action NextTurn { get; set; }

        public TurnBasedPlayerBehaviour(Energy energy, UserInput userInput, List<IDisplayable> combatUI)
        {
            m_energy = energy;
            m_userInput = userInput;
            m_combatUI = combatUI;
        }

        public void Enable()
        {
            m_userInput.Controls.TurnBasedPlayer.Enable();
        }

        public void Disable()
        {
            m_userInput.Controls.TurnBasedPlayer.Disable();
        }

        public void Update()
        {

        }

        public void StartTurn()
        {
            m_energy.Regenerate();

            Debug.Log("Player is doing a move");
            foreach (IDisplayable ui in m_combatUI)
                ui.Show();
        }

        public void EndTurn()
        {
            Debug.Log("Player ended his move");

            foreach (IDisplayable ui in m_combatUI)
                ui.Hide();

            NextTurn?.Invoke();
        }
    }
}