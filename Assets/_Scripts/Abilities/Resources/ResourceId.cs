using UnityEngine;

namespace MagmaHeart.Abilities.Resources
{
    [CreateAssetMenu(menuName = "Actions/Resources/Resource Id")]
    public class ResourceId : ScriptableObject
    {
        [SerializeField] private string m_id;

        public string Id => m_id;
    }
}
