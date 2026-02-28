using MagmaHeart.Abilities;
using UnityEngine;

namespace MagmaHeart.Core.Abilities
{
    [CreateAssetMenu(menuName = "Magma Heart Data/Parameter Database")]
    public class ParameterDatabase : ScriptableObject
    {
        [SerializeField] private ParameterId m_strength;
        [SerializeField] private ParameterId m_speed;
        [SerializeField] private ParameterId m_energy;
        [SerializeField] private ParameterId m_health;

        public ParameterId Strength => m_strength;
        public ParameterId Speed => m_speed;
        public ParameterId Energy => m_energy;
        public ParameterId Health => m_health;
    }
}
