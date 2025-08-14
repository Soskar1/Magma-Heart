using MagmaHeart.Core.Entities.PlayableCharacters;
using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.UI
{
    public class CombatUI : MonoBehaviour, IDisplayable
    {
        [SerializeField] private Button m_nextTurnButton;

        public void Initialize(Player player) => m_nextTurnButton.onClick.AddListener(player.TurnBasedPlayerBehaviour.EndTurn);
        public void Show() => m_nextTurnButton.gameObject.SetActive(true);
        public void Hide() => m_nextTurnButton.gameObject.SetActive(false);
    }
}

