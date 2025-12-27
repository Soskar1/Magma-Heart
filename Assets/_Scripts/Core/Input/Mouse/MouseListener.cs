using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MagmaHeart.Core.Input
{
    public class MouseListener : MonoBehaviour
    {
        private UserInput m_userInput;
        private Vector2 m_currentMouseScreenPosition;
        private Vector2 m_currentWorldMousePosition;

        public event EventHandler<OnMouseWorldPositionChangedEventArgs> OnMouseWorldPositionChanged;
        public event Action OnGameLeftMouseButtonClick;

        public void Initialize(UserInput userInput)
        {
            m_userInput = userInput;

            m_userInput.OnMousePositionChanged += HandleOnMousePositionChanged;
            m_userInput.OnLeftMouseButtonClick += HandleOnLeftMouseButtonClick;
        }

        public void Disable()
        {
            m_userInput.OnMousePositionChanged -= HandleOnMousePositionChanged;
            m_userInput.OnLeftMouseButtonClick -= HandleOnLeftMouseButtonClick;
        }

        public void OnDisable() => Disable();

        private void HandleOnMousePositionChanged(object obj, OnMousePositionChangedEventArgs args) => m_currentMouseScreenPosition = args.Position;

        private void Update()
        {
            if (Camera.main == null)
                return;

            Vector2 world = Camera.main.ScreenToWorldPoint(m_currentMouseScreenPosition);

            if (world != m_currentWorldMousePosition)
            {
                m_currentWorldMousePosition = world;
                OnMouseWorldPositionChangedEventArgs args = new OnMouseWorldPositionChangedEventArgs(world);
                OnMouseWorldPositionChanged?.Invoke(this, args);
            }
        }

        private void HandleOnLeftMouseButtonClick(object obj, EventArgs args) => StartCoroutine(HandleOnLeftMouseButtonClick());

        // Workaround to remove the warning IsPointerOverGameObject called inside the new input system
        // Reference: https://discussions.unity.com/t/ispointerovergameobject-inputaction-callback/946169/5
        private IEnumerator HandleOnLeftMouseButtonClick()
        {
            yield return new WaitForEndOfFrame();

            if (!EventSystem.current.IsPointerOverGameObject())
                OnGameLeftMouseButtonClick?.Invoke();
        }
    }
}

