using MagmaHeart.Abilities;
using MagmaHeart.Core.Artifacts.StatModifiers;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Artifacts
{
    [System.Serializable]
    public class ArtifactLevelDefinition
    {
        [SerializeReference, SubclassSelector]
        private List<IStatModifier> m_statModifiers;

        public IReadOnlyList<IStatModifier> StatModifiers => m_statModifiers;
    }
}
