using MagmaHeart.Core.Artifacts.StatModifiers;
using MagmaHeart.Core.Entities;
using System;

namespace MagmaHeart.Core.Artifacts
{
    public class Artifact
    {
        private int m_currentLevel;

        public ArtifactData Data { get; init; }
        public int CurrentLevel => m_currentLevel;
        public bool IsMaxLevel => m_currentLevel >= Data.MaxLevel;

        public event EventHandler<OnArtifactLeveledUpEventArgs> OnArtifactLeveledUp;

        public Artifact(ArtifactData data)
        {
            Data = data;
            m_currentLevel = 1;
        }

        public void Revert(EntityModel entity)
        {
            foreach (IStatModifier modifier in Data.StatModifiers[m_currentLevel - 1])
                modifier.Revert(entity);
        }

        public void Apply(EntityModel entity)
        {
            foreach (IStatModifier modifier in Data.StatModifiers[m_currentLevel - 1])
                modifier.Apply(entity);
        }

        public void LevelUp()
        {
            if (m_currentLevel >= Data.MaxLevel)
                return;

            ++m_currentLevel;

            OnArtifactLeveledUpEventArgs args = new OnArtifactLeveledUpEventArgs(this);
            OnArtifactLeveledUp?.Invoke(this, args);
        }
    }
}
