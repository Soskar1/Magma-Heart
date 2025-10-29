using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class TacticianAITests
    {
        private Entity m_player;
        private Entity m_enemy1;
        private Entity m_enemy2;
        private Entity m_enemy3;

        public void SetUp()
        {
            m_player = new Entity(10, new Vector2(5, 5), true);
            Func<StateSnapshot, AIUnit> enemySelection = (state) =>
            {
                AIUnit nearestUnit = null;
                float minDistance = float.MaxValue;

                List<AIUnit> allUnits = state.GetAllUnits();
                foreach (AIUnit unit in allUnits)
                {
                    if (unit.IsPlayer)
                        continue;

                    IsAliveProperty isAlive = (IsAliveProperty)state.GetProperty(unit, typeof(IsAliveProperty));
                    if (!isAlive)
                        continue;

                    Position unitPosition = (Position)state.GetProperty(unit, typeof(Position));

                    float distance = unitPosition.Distance(m_player.Position);
                    if (distance < minDistance)
                    {
                        nearestUnit = unit;
                        minDistance = distance;
                    }
                }

                return nearestUnit;
            };

            m_enemy1 = new Entity(4, new Vector2(0, 0), false);
            m_enemy2 = new Entity(4, new Vector2(2, 2), false);
            m_enemy3 = new Entity(4, new Vector2(9, 9), false);
        }



        //[Test]
        //public void TacticianAI_ChooseBestMoveDepth1_ChoosesBestMoveBetweenMultipleActions()
        //{
        //    TacticianAI tactician = new TacticianAI(1, player, enemySelection);

        //    CircularList<AIUnit> circularList = new CircularList<AIUnit>() { enemy1, enemy2, enemy3, player };
        //    ChainNode<AIUnit> moveOrder = (ChainNode<AIUnit>)circularList;

        //    StateSnapshot state = StateSnapshotMaker.CreateStateSnapshot(circularList);
        //    tactician.ChooseBestMove(moveOrder, state);
        //}
    }
}
