using MagmaHeart.AI.Boards;
using MagmaHeart.Core.BoardStateSystem;
using NUnit.Framework;
using UnityEngine;

namespace MagmaHeart.Core.Tests
{
    public class CoreTests
    {
        public CombatBoardState State { get; private set; }
        private BoardDimensions m_boardDimensions;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            m_boardDimensions = new BoardDimensions(Vector2Int.zero, new Vector2Int(10, 10));
        }

        [SetUp]
        public void SetUp()
        {
            Board board = BoardPresets.CreateEmptyBoard(m_boardDimensions);
            Room room = new Room(null, null, null, board.Graph);
            State = new CombatBoardState(room);
        }
    }
}