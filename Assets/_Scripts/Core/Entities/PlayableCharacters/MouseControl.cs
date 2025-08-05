using System;
using MagmaHeart.Core.Dungeon;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class MouseControl
    {
        private UserInput m_userInput;
        private DungeonGrid m_grid;

        private Vector3Int? m_currentMouseTile;
        public Action<Vector3Int> OnMouseChangedTile;
        public Action OnMouseClicked;

        public MouseControl(UserInput userInput, DungeonGrid grid)
        {
            m_userInput = userInput;
            m_grid = grid;
        }

        public void Enable()
        {
            m_userInput.Controls.TurnBasedPlayer.MouseClick.performed += OnMouseClick;
        }

        public void Disable()
        {
            m_userInput.Controls.TurnBasedPlayer.MouseClick.performed -= OnMouseClick;
        }

        public void UpdateMousePosition()
        {
            Vector2 mouseWorldPosition = GetMouseWorldPosition();
            Vector3Int mouseOverTilePosition = m_grid.WorldToTilePosition(mouseWorldPosition);

            if (m_currentMouseTile.HasValue)
            {
                if (m_currentMouseTile.Value != mouseOverTilePosition)
                {
                    m_currentMouseTile = mouseOverTilePosition;
                    OnMouseChangedTile?.Invoke(mouseOverTilePosition);
                }
            }
            else
            {
                m_currentMouseTile = mouseOverTilePosition;
                OnMouseChangedTile?.Invoke(mouseOverTilePosition);
            }
        }

        private Vector2 GetMouseWorldPosition()
        {
            Vector2 currentMousePosition = m_userInput.Controls.TurnBasedPlayer.MousePosition.ReadValue<Vector2>();
            return Camera.main.ScreenToWorldPoint(currentMousePosition);
        }

        private void OnMouseClick(InputAction.CallbackContext context)
        {
            OnMouseClicked?.Invoke();
        }
    }
}