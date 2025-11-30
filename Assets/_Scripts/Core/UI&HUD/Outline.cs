using MagmaHeart.Core.Input;
using System;
using UnityEngine;

namespace MagmaHeart.Core.UI
{
    [RequireComponent(typeof(MouseHover))]
    public class Outline : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_renderer;
        [SerializeField] private Material m_outline;
        private Material m_defaultMaterial;
        private MouseHover m_mouseHover;

        private void Awake()
        {
            m_mouseHover = GetComponent<MouseHover>();
            m_defaultMaterial = m_renderer.material;

            m_mouseHover.OnMouseEnterEvent += HandleOnMouseEnter;
            m_mouseHover.OnMouseExitEvent += HandleOnMouseExit;
        }

        private void OnDisable()
        {
            m_mouseHover.OnMouseEnterEvent -= HandleOnMouseEnter;
            m_mouseHover.OnMouseExitEvent -= HandleOnMouseExit;
        }

        private void HandleOnMouseEnter(object obj, EventArgs args)
        {
            m_renderer.material = m_outline;
        }

        private void HandleOnMouseExit(object obj, EventArgs args)
        {
            m_renderer.material = m_defaultMaterial;
        }
    }
}
