using MagmaHeart.Core.Entities;
using MagmaHeart.DungeonGeneration;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MagmaHeart.Core.Input.Mouse
{
    public class MouseHover : IDisposable
    {
        private readonly MouseListener m_listener;
        private readonly GameWorld m_world;
        private readonly GraphicRaycaster m_uiRaycaster;

        public event EventHandler<OnHoverResultChangedEventArgs> OnHoverResultChanged;

        public MouseHover(MouseListener listener, GameWorld world, GraphicRaycaster uiRaycaster)
        {
            m_listener = listener;
            m_uiRaycaster = uiRaycaster;
            m_world = world;
            m_listener.OnMouseWorldPositionChanged += HandleMousePositionChanged;
        }

        public void Dispose()
        {
            m_listener.OnMouseWorldPositionChanged -= HandleMousePositionChanged;
        }

        private void HandleMousePositionChanged(object _, OnMouseWorldPositionChangedEventArgs args) => Hover(args.WorldPosition);

        public void Hover(Vector2 worldPosition)
        {
            HoverResult hoverResult = GetHoverResult(worldPosition);

            OnHoverResultChangedEventArgs hoverArgs = new OnHoverResultChangedEventArgs(hoverResult);
            OnHoverResultChanged?.Invoke(this, hoverArgs);
        }

        private HoverResult GetHoverResult(Vector2 worldPosition)
        {
            HoverResult hoverResult = HoverResult.Empty(worldPosition);
            hoverResult = TryHoverUI(worldPosition, hoverResult);
            hoverResult = TryHoverEntity(worldPosition, hoverResult);
            hoverResult = TryHoverTile(worldPosition, hoverResult);
            
            return hoverResult;
        }

        private HoverResult TryHoverUI(Vector2 worldPosition, HoverResult currentHover)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = Camera.main.WorldToScreenPoint(worldPosition)
            };

            List<RaycastResult> results = new();
            m_uiRaycaster.Raycast(eventData, results);

            if (results.Count == 0)
                return currentHover;

            foreach (RaycastResult raycastResult in results)
            {
                UIMouseInteraction detection = raycastResult.gameObject.GetComponentInParent<UIMouseInteraction>();
                if (detection != null)
                    return currentHover.AppendUIInfo(detection.gameObject);
            }

            return currentHover;
        }

        private HoverResult TryHoverEntity(Vector2 worldPosition, HoverResult currentHover)
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector3.back, Mathf.Infinity);

            if (hit.collider != null && hit.collider.TryGetComponent(out Entity entity))
                return currentHover.AppendEntityInfo(entity);

            return currentHover;
        }

        private HoverResult TryHoverTile(Vector2 worldPosition, HoverResult currentHover)
        {
            DungeonTile tile = m_world.GetTile(worldPosition);

            if (tile == null)
                return currentHover;

            currentHover.AppendTileInfo(tile);
            
            if (m_world.TryGetEntityAtPosition(worldPosition, out Entity entity))
                currentHover.AppendEntityInfo(entity);

            return currentHover;
        }
    }
}
