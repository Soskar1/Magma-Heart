using System;
using MagmaHeart.Core.Dungeon;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class MouseControl
    {
        private UserInput m_userInput;
        private GameGrid m_grid;

        private Vector3Int? m_currentMouseTile;
        public Action<Vector3Int> OnMouseChangedTile;

        public Vector3Int? CurrentMouseTile => m_currentMouseTile;

        public MouseControl(UserInput userInput, GameGrid grid)
        {
            m_userInput = userInput;
            m_grid = grid;
        }

        public void UpdateMousePosition()
        {
            Vector2 currentMousePosition = m_userInput.Controls.TurnBasedPlayer.MousePosition.ReadValue<Vector2>();
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(currentMousePosition);
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

        public void ClearMousePosition() => m_currentMouseTile = null;
    }
}