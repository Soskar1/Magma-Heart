using System;
using System.Reflection;
using MagmaHeart.Abilities;
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
        public ParameterDatabase TestDatabase { get; private set; }
        private BoardDimensions m_boardDimensions;

        [OneTimeSetUp]
        public void InitializeBoardDimensions()
        {
            m_boardDimensions = new BoardDimensions(Vector2Int.zero, new Vector2Int(10, 10));
        }

        [SetUp]
        public void SetUp()
        {
            RoomModel roomModel = RoomPresets.CreateEmptyRoom(m_boardDimensions);
            Room room = new Room(roomModel, null, null);
            World = new TestGameWorld(room);
            TestDatabase = CreateTestDatabase();
        }

        private static ParameterDatabase CreateTestDatabase()
        {
            ParameterDatabase database = AssetDatabase.LoadAssetAtPath<ParameterDatabase>("Assets/Data/Abilities/Resources/Parameter Database.asset");
            return database;
        }

        public static EffectDispatcher CreateDispatcher()
        {
            EffectDispatcher dispatcher = new EffectDispatcher();
            dispatcher.Register(new DamageHandler());
            dispatcher.Register(new SpendResourceHandler());
            dispatcher.Register(new RestoreParameterHandler());
            dispatcher.Register(new MoveHandler());
            return dispatcher;
        }
    }
}