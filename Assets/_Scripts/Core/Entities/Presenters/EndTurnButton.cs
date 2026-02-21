using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Presentation.UI;
using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.Entities.Presenters
{
    public class EndTurnButton : MonoBehaviour, IDisplayable
    {
        [SerializeField] private Button m_nextTurnButton;
        private PlayerTurnController m_turnController;

        public void Initialize(PlayerTurnController playerTurnController)
        {
            m_turnController = playerTurnController;
            m_nextTurnButton.onClick.AddListener(playerTurnController.EndTurn);

            m_turnController.OnCanExecuteActionsChanged += HandleOnCanExecuteActionsChanged;
        }

        private void OnDisable() => m_turnController.OnCanExecuteActionsChanged -= HandleOnCanExecuteActionsChanged;

        private void HandleOnCanExecuteActionsChanged(object obj, OnCanExecuteActionsChangedEventArgs args) => m_nextTurnButton.enabled = args.CanExecute;

        public void Show() => m_nextTurnButton.gameObject.SetActive(true);
        public void Hide() => m_nextTurnButton.gameObject.SetActive(false);
    }
}

