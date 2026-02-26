using MagmaHeart.Abilities;
using MagmaHeart.AI.Boards;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Plans
{
    [CreateAssetMenu(menuName = "AI/Plan Definition")]
    public class PlanDefinition : ScriptableObject
    {
        [SerializeField] private List<PlanTask> m_taskDefinitions;

        public List<PlanTask> TaskDefinitions => m_taskDefinitions;
    }

    [System.Serializable]
    public class PlanTask
    {
        [SerializeField] private AbilityDefinition m_ability;
        [SerializeField] private bool m_executeUntilFail;

        public AbilityDefinition Ability => m_ability;
        public bool ExecuteUntilFail => m_executeUntilFail;

        public bool TryExecute(Board simulation, AIUnitModel executor, out AbilityPlan abilityPlan)
        {
            abilityPlan = null;

            //if (!Action.TryGenerateArgs(executor, ActionDefinition.Data, simulation, out ActionArgs args))
            //    return false;

            //IEnumerable<IBoardCommand> commands = Action.Execute(args, simulation);
            //runner.Apply(simulation, commands);

            //abilityPlan = new ExecutedTask(Action, args);
            return true;
        }
    }
}
