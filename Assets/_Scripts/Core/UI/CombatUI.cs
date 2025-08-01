using MagmaHeart.Core.Entities.PlayableCharacters;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MagmaHeart.Core.UI
{
    public class CombatUI : MonoBehaviour
    {
        [SerializeField] private Button m_nextTurnButton;

        public void Initialize(Player player)
        {
            m_nextTurnButton.onClick.AddListener(player.TurnBasedPlayerBehaviour.EndTurn);
        }
    }
}

