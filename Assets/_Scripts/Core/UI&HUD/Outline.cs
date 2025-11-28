using MagmaHeart.Core.Input;
using System;
using UnityEngine;

namespace MagmaHeart.Core.UI
{
    [RequireComponent(typeof(MouseHover))]
    public class Outline : MonoBehaviour
    {
        [SerializeField] private Color m_outline;
        private MouseHover m_mouseHover;

        private void Awake()
        {
            m_mouseHover = GetComponent<MouseHover>();

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
            Debug.Log("Show outline!");
        }

        private void HandleOnMouseExit(object obj, EventArgs args)
        {
            Debug.Log("Hide outline!");
        }
    }
}
