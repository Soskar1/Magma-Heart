using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.Entities.Presenters
{
    public class EndTurnButton : MonoBehaviour, IDisplayable
    {
        [SerializeField] private Button m_nextTurnButton;
        private PlayerTurnContext m_turnContext;

        public void Initialize(Player player)
        {
            m_turnContext = (PlayerTurnContext)player.TurnContext;
            m_nextTurnButton.onClick.AddListener(player.TurnContext.EndTurn);

            m_turnContext.OnCanExecuteActionsChanged += HandleOnCanExecuteActionsChanged;
        }

        private void OnDisable() => m_turnContext.OnCanExecuteActionsChanged -= HandleOnCanExecuteActionsChanged;

        private void HandleOnCanExecuteActionsChanged(object obj, OnCanExecuteActionsChangedEventArgs args) => m_nextTurnButton.enabled = args.CanExecute;

        public void Show() => m_nextTurnButton.gameObject.SetActive(true);
        public void Hide() => m_nextTurnButton.gameObject.SetActive(false);
    }
}

