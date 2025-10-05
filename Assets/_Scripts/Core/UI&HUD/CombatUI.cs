using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.StateMachines;
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

        public void Show() => m_nextTurnButton.gameObject.SetActive(true);
        public void Hide() => m_nextTurnButton.gameObject.SetActive(false);

        public void HandleOnTurnSwitched(object obj, OnTurnSwitchedEventArgs args)
        {
            if (args.Entity.IsPlayableCharacter)
                Show();
            else
                Hide();
        }

        public void EnterCombatState() { }
        public void ExitCombatState() => Hide();
    }
}

