using MagmaHeart.AI.Boards;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.Dungeon;
using NUnit.Framework;
using UnityEngine;

namespace MagmaHeart.Core.Tests
{
    public class CoreTests
    {
        public CombatBoardState State { get; private set; }
        private BoardDimensions m_boardDimensions;

        [OneTimeSetUp]
        public void InitializeBoardDimensions()
        {
            m_boardDimensions = new BoardDimensions(Vector2Int.zero, new Vector2Int(10, 10));
        }

        [SetUp]
        public void SetUp()
        {
            Board board = BoardPresets.CreateEmptyBoard(m_boardDimensions);
            //Room room = new Room(null, null, null);
            //State = new CombatBoardState(room);
            throw new System.Exception("FIX THIS");
        }
    }
}