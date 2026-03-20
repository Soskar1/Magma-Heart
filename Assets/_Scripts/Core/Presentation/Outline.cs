using UnityEngine;

namespace MagmaHeart.Core.Presentation
{
    public class Outline : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_renderer;
        private Material m_defaultMaterial;

        private void Awake() => m_defaultMaterial = m_renderer.material;
        
        public void ApplyOutline(Material outlineMaterial)
        {
            m_renderer.material = outlineMaterial;
            
            // Without this, sprites will disappear
            m_renderer.material.SetTexture("_MainTex", m_renderer.sprite.texture);
        }

        public void RemoveOutline()
        {
            m_renderer.material = m_defaultMaterial;
        }
    }
}
