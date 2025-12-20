using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.AI.Reasoning.Plans;
using MagmaHeart.AI.States;
using MagmaHeart.Collections;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.CombatSystem;

namespace MagmaHeart.Core.Entities.NonPlayableCharacters
{
    public class CombatAI
    {
        private readonly AIEngine m_tactician;
        private readonly Plan m_doNothing;
        private TurnOrder m_currentTurnOrder;
        private CombatBoardState m_currentBoardState;

        public CombatAI(Strategy strategy, ActionDatabase database, int lookAhead) {
            m_tactician = new AIEngine(strategy, database, lookAhead);

            PlanTask doNothingTask = new PlanTask(new DoNothingAction());
            m_doNothing = new Plan(doNothingTask);
        }

        public BestPlan GetBestAction()
        {
            ChainNode<TurnContext> chain = m_currentTurnOrder.ToChainNode();
            BestPlan bestAction = m_tactician.ChooseBestMove(chain, m_currentBoardState);

            if (bestAction == null)
            {
                bestAction = new BestPlan(
                    m_doNothing,
                    new ActionArgs(m_currentTurnOrder.Current.Model));
            }

            return bestAction;
        }

        public void HandleOnBattleStarted(object obj, OnBattleStartedEventArgs args)
        {
            m_currentTurnOrder = args.TurnOrder;
            m_currentBoardState = args.CombatBoardState;
        }
    }
}
