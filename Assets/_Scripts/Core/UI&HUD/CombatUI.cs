using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.StateMachines;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.UI
{
    public class CombatUI : MonoBehaviour, IDisplayable, ICombatStateListener, ICombatTurnSwitchListener
    {
        [SerializeField] private Button m_nextTurnButton;

        public void Initialize(Player player)
        {
            m_nextTurnButton.onClick.AddListener(player.TurnBasedPlayerBehaviour.EndTurn);
        }

        private void Enable() => m_nextTurnButton.enabled = true;
        private void Disable() => m_nextTurnButton.enabled = false;

        public void Show() => m_nextTurnButton.gameObject.SetActive(true);
        public void Hide() => m_nextTurnButton.gameObject.SetActive(false);

        public void EnterCombatState() { }
        public void ExitCombatState() => Hide();

        public void HandleOnTurnSwitched(object obj, OnTurnSwitchedEventArgs args)
        {
            if (args.Entity.IsPlayableCharacter)
                Show();
            else
                Hide();
        }

        public void HandleOnMovementStarted(object obj, OnMovementEventArgs args) => Disable();
        public void HandleOnMovementEnded(object obj, OnMovementEventArgs args) => Enable();
        public void HandleOnAttackTriggered(object obj, OnAttackEventArgs args) => Disable();
        public void HandleOnAttackAnimationEnded(object obj, EventArgs args) => Enable();
    }
}

