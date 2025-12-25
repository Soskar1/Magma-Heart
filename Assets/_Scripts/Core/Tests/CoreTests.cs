using System.Collections.Generic;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.BoardStateSystem.Actions.Data;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using MagmaHeart.Extensions;
using NUnit.Framework;
using UnityEngine;

namespace MagmaHeart.Core.Tests
{
    internal class CoreTests
    {
        private CombatBoardState m_state;
        private BoardDimensions m_boardDimensions;

        public CombatBoardState State => m_state;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            m_boardDimensions = new BoardDimensions(Vector2Int.zero, new Vector2Int(10, 10));
        }

        [SetUp]
        public void SetUp()
        {
            Board board = BoardBuilder.CreateEmptyBoard(m_boardDimensions);
            Room room = new Room(null, null, null, board.Graph);
            m_state = new CombatBoardState(room);
        }

        public TurnContext<EntityModel> AddEntity(Vector3Int position, bool isPlayer, int maxHealth = 5)
        {
            EntityStats stats = new EntityStats(maxHealth);
            List<ActionData> actions = new List<ActionData>
            {
                new AttackActionData(2, 1, 1, AttackType.Melee),
                new MovementActionData(2)
            };
            EntityData data = new EntityData("", stats, actions);
            EntityModel model = new EntityModel(data, () => position, isPlayer);
            m_state.Room.AddUnit(position.ToVector2(), model);
            m_state.Room.ChangeNodeType(position.ToVector2(), BoardNodeType.Obstacle);

            return new EnemyCombatController(model, null);
        }
    }
}
