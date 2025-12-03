using UnityEngine;

namespace MagmaHeart.Core.Presentation
{
    public class Outline : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_renderer;
        [SerializeField] private Material m_outline;
        private Material m_defaultMaterial;

        private void Awake() => m_defaultMaterial = m_renderer.material;
        
        public void ApplyOutline(Color color)
        {
            m_outline.color = color;
            m_renderer.material = m_outline;
        }

        public void RemoveOutline()
        {
            m_outline.color = Color.white;
            m_renderer.material = m_defaultMaterial;
        }
    }
}
