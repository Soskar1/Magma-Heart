using UnityEngine;

namespace MagmaHeart.Abilities.Resources
{
    [CreateAssetMenu(menuName = "Abilities/Resources/Resource Id")]
    public class ResourceId : ScriptableObject
    {
        [SerializeField] private string m_id;

        public string Id => m_id;
    }
}
