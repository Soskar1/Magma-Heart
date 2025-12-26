using MagmaHeart.AI.Boards;
using UnityEngine;

namespace MagmaHeart.Core.Tests
{
    internal sealed class BoardBuilder
    {
        private readonly AIScenarioBuilder m_scenario;
        private Board Board => m_scenario.Board;

        public BoardBuilder(AIScenarioBuilder scenario) => m_scenario = scenario;

        public BoardBuilder PlaceWallAt(int x, int y)
        {
            Board.ChangeNodeType(new Vector2(x, y), BoardNodeType.Obstacle);
            return this;
        }

        public BoardBuilder SurroundWithWalls(int xCenter, int yCenter)
        {
            for (int x = -1; x <= 1; ++x)
            {
                for (int y = -1; y <= 1; ++y)
                {
                    if (x == 0 && y == 0)
                        continue;

                    Vector2 wallPosition = new Vector2(xCenter + x, yCenter + y);
                    Board.ChangeNodeType(wallPosition, BoardNodeType.Obstacle);
                }
            }

            return this;
        }

        public AIScenarioBuilder Bake() => m_scenario;
    }
}
