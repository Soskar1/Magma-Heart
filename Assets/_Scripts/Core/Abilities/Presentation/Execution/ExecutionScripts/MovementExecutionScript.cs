using MagmaHeart.Abilities;
using MagmaHeart.Core.Abilities.Effects;
using MagmaHeart.Core.Abilities.Presentation.Execution.Steps;
using MagmaHeart.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.ExecutionScripts
{
    [CreateAssetMenu(menuName = "Magma Heart Data/Ability Execution/Movement")]
    public class MovementExecutionScript : AbilityExecutionScript
    {
        public override List<IAbilityExecutionStep> BuildSteps(AbilityPlan plan, int executorId)
        {
            List<MoveEffect> moveEffects = plan.Effects.Where(effect => effect is MoveEffect).Cast<MoveEffect>().ToList();

            List<IAbilityExecutionStep> steps = new List<IAbilityExecutionStep>
            {
                new PlayAnimationStep(AnimationType.Run),
                new MoveEntityStep(moveEffects),
                new ApplyEffectsStep(plan.Effects),
                new PlayAnimationStep(AnimationType.Idle)
            };

            return steps;
        }
    }
}
