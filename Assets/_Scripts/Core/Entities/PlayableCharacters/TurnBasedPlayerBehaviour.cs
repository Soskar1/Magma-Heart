using System;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.UI;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class TurnBasedPlayerBehaviour : IPlayerBehaviour, ITurnController
    {
        private UserInput m_userInput;
        private CombatUI m_combatUI;

        public Action NextTurn { get; set; }

        public TurnBasedPlayerBehaviour(UserInput userInput, CombatUI combatUI)
        {
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
            Debug.Log("Player is doing a move");
            m_combatUI.gameObject.SetActive(true);
        }

        public void EndTurn()
        {
            Debug.Log("Player ended his move");
            m_combatUI.gameObject.SetActive(false);
            NextTurn?.Invoke();
        }
    }
}