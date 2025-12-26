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
            m_cancellationTokenSource = new CancellationTokenSource();
            await StartTurnAsync(CurrentCombatBoardState, m_cancellationTokenSource.Token);

            BestPlan best = m_ai.GetBestAction();

            if (best != null)
                foreach (ExecutedTask task in best.ExecutedTasks)
                    await task.Action.ExecuteAsync(task.Args, CurrentCombatBoardState, m_cancellationTokenSource.Token);

            EndTurn();
        }

        public override void EndBattle()
        {
            base.EndBattle();
            m_cancellationTokenSource.Cancel();
        }

        public override void EndTurn()
        {
            if (!m_cancellationTokenSource.IsCancellationRequested)
                m_cancellationTokenSource.Cancel();
        }
    }
}
