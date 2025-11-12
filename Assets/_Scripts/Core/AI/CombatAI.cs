using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Collections;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.AI
{
    public class CombatAI : ICombatTurnSwitchListener, IBattleStartedListener
    {
        private readonly TacticianAI m_tactician;
        private Room m_room;

        public CombatAI(Strategy strategy) => m_tactician = new TacticianAI(strategy);

        public void HandleOnBattleStarted(object obj, OnBattleStartedEventArgs args) => m_room = args.Room;

        // TODO: Move to the AICombatController
        public void HandleOnTurnSwitched(object obj, OnTurnSwitchedEventArgs args)
        {
            if (args.CurrentEntity.Model.IsPlayer)
                return;

            args.CurrentEntity.Model.Energy.Regenerate();

            List<AIUnit> units = args.CurrentTurnOrder.Select(x => (AIUnit)x.Model).ToList();
            CircularList<AIUnit> turnOrder = new CircularList<AIUnit>();

            foreach (AIUnit unit in units)
                turnOrder.Add(unit);

            Debug.Log($"{args.CurrentEntity.gameObject.name} {args.CurrentEntity.transform.position} Searching for the best move...");
            BestAction bestAction = m_tactician.ChooseBestMove(turnOrder, m_room);

            if (bestAction == null)
            {
                Debug.LogWarning("bestAction is null");
                return;
            }

            bestAction.Action.Execute(bestAction.Args);
            Debug.Log("AI has performed its move.");
        }
    }
}
