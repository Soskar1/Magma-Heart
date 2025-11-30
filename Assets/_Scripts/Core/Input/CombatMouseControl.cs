using System;
using System.Collections.Generic;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MagmaHeart.Core.Input
{
    public class CombatMouseControl
    {
        private readonly CombatUserInput m_userInput;
        private readonly DungeonGrid m_grid;
        private readonly List<MouseOverUIElement> m_uiElementEvents;

        private Vector3Int? m_currentMouseTile;
        private bool m_mouseOverUIElement = false;

        public event EventHandler<OnMouseChangedTileEventArgs> OnMouseChangedTile;
        public event EventHandler<OnMouseClickedEventArgs> OnMouseClicked;

        public CombatMouseControl(CombatUserInput userInput, DungeonGrid grid, List<MouseOverUIElement> uiElementEvents)
        {
            m_userInput = userInput;
            m_grid = grid;
            m_uiElementEvents = uiElementEvents;
        }

        public void Enable()
        {
            foreach (MouseOverUIElement uiElementEvent in m_uiElementEvents)
            {
                uiElementEvent.OnMouseEnterUIElement += HandleOnMouseEnterUIElement;
                uiElementEvent.OnMouseExitUIElement += HandleOnMouseExitUIElement;
            }

            // m_userInput.TurnBasedPlayer.MouseClick.performed += OnMouseClick;
        }

        public void Disable()
        {
            foreach (MouseOverUIElement uiElementEvent in m_uiElementEvents)
            {
                uiElementEvent.OnMouseEnterUIElement -= HandleOnMouseEnterUIElement;
                uiElementEvent.OnMouseExitUIElement -= HandleOnMouseExitUIElement;
            }

            // m_userInput.TurnBasedPlayer.MouseClick.performed -= OnMouseClick;
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
            //Vector2 currentMousePosition = m_userInput.TurnBasedPlayer.MousePosition.ReadValue<Vector2>();
            //return Camera.main.ScreenToWorldPoint(currentMousePosition);
            return Vector2.zero;
        }

        private void OnMouseClick(InputAction.CallbackContext context)
        {
            OnMouseClickedEventArgs args = new OnMouseClickedEventArgs(m_mouseOverUIElement);
            OnMouseClicked?.Invoke(this, args);
        }

        private void HandleOnMouseEnterUIElement(object obj, EventArgs args) => m_mouseOverUIElement = true;
        private void HandleOnMouseExitUIElement(object obj, EventArgs args) => m_mouseOverUIElement = false;
    }
}