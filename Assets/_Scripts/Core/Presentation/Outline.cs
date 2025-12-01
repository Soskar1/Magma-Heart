using UnityEngine;

namespace MagmaHeart.Core.Presentation
{
    [RequireComponent(typeof(MouseHover))]
    public class Outline : MonoBehaviour, IHoverable
    {
        [SerializeField] private SpriteRenderer m_renderer;
        [SerializeField] private Material m_outline;
        private Material m_defaultMaterial;

        private void Awake() => m_defaultMaterial = m_renderer.material;
        public void ApplyHover() => m_renderer.material = m_outline;
        public void UndoHover() => m_renderer.material = m_defaultMaterial;
    }
}
