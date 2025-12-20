using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.AI.Reasoning.Plans;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Entities.NonPlayableCharacters
{
    public class EnemyCombatController : EntityTurnContext
    {
        private readonly CombatAI m_ai;
        private CancellationTokenSource m_cancellationTokenSource;

        public EnemyCombatController(EntityModel model, CombatAI ai) : base(model)
        {
            m_ai = ai;
        }

        public override async Task StartTurnTask()
        {
            base.StartTurnTask();

            m_cancellationTokenSource = new CancellationTokenSource();

            BestPlan best = m_ai.GetBestAction();

            if (best != null)
            {
                foreach (PlanTask task in best.ExecutedTasks)
                {
                    ActionArgs args = task.ActionDefinition.CreateArguments(Model, best.Target, CurrentCombatBoardState);
                    await task.Action.ExecuteAsync(args, CurrentCombatBoardState, m_cancellationTokenSource.Token);
                }
            }

            EndTurn();
        }

        public override void EndBattle()
        {
            base.EndBattle();
            m_cancellationTokenSource.Cancel();
        }
    }
}
