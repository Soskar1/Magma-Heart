using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Artifacts
{
    public record ArtifactData(string Name, string Description, Rarity Rarity, Sprite Icon, List<List<IStatModifier>> StatModifiers)
    {
        public int MaxLevel => StatModifiers.Count;
    }
}