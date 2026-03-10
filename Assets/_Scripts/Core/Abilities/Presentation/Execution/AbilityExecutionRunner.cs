using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Effects;
using MagmaHeart.AI;
using MagmaHeart.Core.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Abilities.Presentation.Execution
{
    public class AbilityExecutionRunner
    {
        private readonly AbilityExecutionScriptDatabase m_database;
        private readonly EffectDispatcher m_effectDispatcher;
        private readonly GameWorld m_world;

        public AbilityExecutionRunner(AbilityExecutionScriptDatabase database, EffectDispatcher effectDispatcher, GameWorld world)
        {
            m_database = database;
            m_effectDispatcher = effectDispatcher;
            m_world = world;
        }

        public async Task Run(AbilityPlan plan, EntityModel executor, CancellationToken cancellationToken)
        {
            executor.SetCooldown(plan.AbilityDefinition.Id, plan.AbilityDefinition.CooldownTurns);

            bool scriptExists = m_database.TryGetValidScript(plan.AbilityDefinition, plan, out AbilityExecutionScript script);

            if (scriptExists)
            {
                var context = new AbilityExecutionContext(m_world, executor.Id, m_effectDispatcher, plan);

                foreach (IAbilityExecutionStep step in script.Steps)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    await step.Run(context, cancellationToken);
                }
            }
            else
            {
                foreach (AbilityEffect effect in plan.Effects)
                    m_effectDispatcher.Apply(m_world, effect);
            }
        }
    }
}
