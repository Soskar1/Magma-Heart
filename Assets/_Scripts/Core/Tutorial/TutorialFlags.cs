using System;

namespace MagmaHeart.Core.TutorialSystem
{
    [Flags]
    public enum TutorialFlags
    {
        None = 0,
        OpenedWelcomeScreen = 1,
        HealthBarExplained = 2,
        CombatSystemExplained = 4,
        ArtifactsExplained = 8
    }
}