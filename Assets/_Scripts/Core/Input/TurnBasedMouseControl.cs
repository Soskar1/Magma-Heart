using System;
using MagmaHeart.Core.Dungeon;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MagmaHeart.Core.Input
{
    public class TurnBasedMouseControl
    {
        private CombatUserInput m_userInput;
        private DungeonGrid m_grid;

        private Vector3Int? m_currentMouseTile;

        public event EventHandler<OnMouseChangedTileEventArgs> OnMouseChangedTile;
        public event EventHandler<EventArgs> OnMouseClicked;

        public TurnBasedMouseControl(CombatUserInput userInput, DungeonGrid grid)
        {
            m_userInput = userInput;
            m_grid = grid;
        }

        public void Enable() => m_userInput.TurnBasedPlayer.MouseClick.performed += OnMouseClick;
        public void Disable() => m_userInput.TurnBasedPlayer.MouseClick.performed -= OnMouseClick;

        public void UpdateMousePosition()
        {
            if (!m_userInput.TurnBasedPlayer.enabled)
                Debug.LogWarning("Combat user input is not enabled. Cannot update mouse position.");

            Vector2 mouseWorldPosition = GetMouseWorldPosition();
            Vector3Int mouseOverTilePosition = m_grid.WorldToTilePosition(mouseWorldPosition);

            if (m_currentMouseTile.HasValue)
            {
                if (m_currentMouseTile.Value != mouseOverTilePosition)
                {
                    m_currentMouseTile = mouseOverTilePosition;
                    OnMouseChangedTile?.Invoke(this, new OnMouseChangedTileEventArgs(mouseOverTilePosition));
                }
            }
            else
            {
                m_currentMouseTile = mouseOverTilePosition;
                OnMouseChangedTile?.Invoke(this, new OnMouseChangedTileEventArgs(mouseOverTilePosition));
            }
        }

        public void ForceTriggerOnMouseChangedTile()
        {
            UpdateMousePosition();
            OnMouseChangedTile?.Invoke(this, new OnMouseChangedTileEventArgs(m_currentMouseTile.Value));
        }

        private Vector2 GetMouseWorldPosition()
        {
            Vector2 currentMousePosition = m_userInput.TurnBasedPlayer.MousePosition.ReadValue<Vector2>();
            return Camera.main.ScreenToWorldPoint(currentMousePosition);
        }

        private void OnMouseClick(InputAction.CallbackContext context) => OnMouseClicked?.Invoke(this, EventArgs.Empty);
    }
}