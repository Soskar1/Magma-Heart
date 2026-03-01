using MagmaHeart.AI;
using MagmaHeart.Core.Abilities;
using MagmaHeart.Core.Abilities.Effects.Handlers;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.DungeonGeneration;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using MagmaHeart.AI.Boards;

namespace MagmaHeart.Core.Tests
{
    public class CoreTests
    {
        public TestGameWorld World { get; private set; }
        public Board Board => World.Board;
        public ParameterDatabase ParameterDatabase { get; private set; }
        public EffectDispatcher Dispatcher { get; private set; }

        private BoardDimensions m_boardDimensions;

        [OneTimeSetUp]
        public void InitializeBoardDimensions()
        {
            m_boardDimensions = new BoardDimensions(Vector2Int.zero, new Vector2Int(10, 10));
            Dispatcher = new EffectDispatcher();
            Dispatcher.Register(new DamageHandler());
            Dispatcher.Register(new SpendResourceHandler());
            Dispatcher.Register(new RestoreParameterHandler());
            Dispatcher.Register(new MoveHandler());
        }

        [SetUp]
        public void SetUp()
        {
            RoomModel roomModel = RoomPresets.CreateEmptyRoom(m_boardDimensions);
            Room room = new Room(roomModel, null, null);
            World = new TestGameWorld(room);
            ParameterDatabase = CreateTestDatabase();
        }

        private static ParameterDatabase CreateTestDatabase()
        {
            ParameterDatabase database = AssetDatabase.LoadAssetAtPath<ParameterDatabase>("Assets/Data/Abilities/Resources/Parameter Database.asset");
            return database;
        }
    }
}