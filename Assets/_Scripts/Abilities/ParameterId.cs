using UnityEngine;

namespace MagmaHeart.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/Stat Id")]
    public class ParameterId : ScriptableObject
    {
        [SerializeField] private string m_id;

        public string Id => m_id;
    }
}
