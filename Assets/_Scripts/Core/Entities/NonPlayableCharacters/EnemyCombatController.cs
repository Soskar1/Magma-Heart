using MagmaHeart.AI.Actions;
using MagmaHeart.Core.Entities.CombatSystem;
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

            BestAction action = m_ai.GetBestAction();
            await action.Action.ExecuteAsync(action.Args, CurrentCombatBoardState, m_cancellationTokenSource.Token);
        }

        public override void EndBattle()
        {
            base.EndBattle();
            m_cancellationTokenSource.Cancel();
        }
    }
}
