using MagmaHeart.Abilities;
using MagmaHeart.Core.Abilities.Presentation.Execution.Steps;
using MagmaHeart.Core.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.ExecutionScripts
{
    [CreateAssetMenu(menuName = "Magma Heart Data/Ability Execution/Melee Attack")]
    public class MeleeAttackExecutionScript : AbilityExecutionScript
    {
        [SerializeField] private float m_timeoutSeconds = 5f;

        public override List<IAbilityExecutionStep> BuildSteps(AbilityPlan plan, int executorId)
        {
            TimeSpan timeout = TimeSpan.FromSeconds(m_timeoutSeconds);

            List<IAbilityExecutionStep> steps = new List<IAbilityExecutionStep>
            {
                new PlayAnimationStep(AnimationType.Attack),
                new WaitForAnimationEventStep(timeout),
                new ApplyEffectsStep(plan.Effects),
                new WaitForAnimationEndStep(AnimationType.Attack, timeout),
                new PlayAnimationStep(AnimationType.Idle)
            };

            return steps;
        }
    }
}
