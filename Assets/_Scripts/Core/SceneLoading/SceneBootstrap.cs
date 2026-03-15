using MagmaHeart.Abilities.Effects;
using MagmaHeart.AI;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.Abilities;
using MagmaHeart.Core.Abilities.Effects.Handlers;
using MagmaHeart.Core.Abilities.Presentation.Execution;
using MagmaHeart.Core.Abilities.Selection;
using MagmaHeart.Core.AI;
using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.CameraControls;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon.Data;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Input;
using MagmaHeart.Core.Input.Mouse;
using MagmaHeart.Core.Presentation.UI;
using MagmaHeart.Core.Presentation.UI.WindowPopupSystem;
using MagmaHeart.Core.Services;
using MagmaHeart.Core.StateMachine;
using MagmaHeart.Core.Statistics;
using MagmaHeart.Core.TutorialSystem;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace MagmaHeart.Core.SceneLoading
{
    public class SceneBootstrap : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private EntityData m_playerData;
        [SerializeField] private CameraController m_cameraPrefab;

        [Header("AI")]
        [SerializeField] private Strategy m_strategy;
        [SerializeField] private int m_lookAhead;

        [Header("Artifacts")]
        [SerializeField] private ArtifactDatabase m_artifactDatabase;

        [Header("Input")]
        [SerializeField] private MouseListener m_mouseListenerPrefab;

        [Header("Actions")]
        [SerializeField] private int m_movementSpeed;

        [Header("Abilities")]
        [SerializeField] private AbilityExecutionScriptDatabase m_scriptDatabase;

        [Header("Parameters")]
        [SerializeField] private ParameterDatabase m_parameterDatabase;

        [Header("Combat")]
        [SerializeField] private int m_energyRegenPerTurn = 5;

        [Header("Dungeon")]
        [SerializeField] private List<LocationData> m_locations;
        [SerializeField] private Tilemap m_dungeonTilemap;
        [SerializeField] private Grid m_dungeonGrid;
        [SerializeField] private WorldPresenter m_worldPresenter;
        [SerializeField] private int m_seed;
        [SerializeField] private TileBase m_floorTile;
        [SerializeField] private TileBase m_wallTile;

        [Header("Spawner Settings")]
        [SerializeField] private Entity m_entityPrefab;
        [SerializeField] private Projectile m_projectilePrefab;
        [SerializeField] private float m_minDistanceFromPlayer;

        [Header("UI")]
        [SerializeField] private AbilitySelectorPresenter m_abilitySelectorPresenter;
        [SerializeField] private MagmaHeartWindowDatabaseDefinition m_windowDatabase;
        [SerializeField] private GameUI m_gameUI;
        [SerializeField] private DebugUI m_debugUI;
        [SerializeField] private EscapeScreen m_escapeScreen;
        [SerializeField] private GraphicRaycaster m_graphicRaycaster;

        [Header("Tutorial")]
        [SerializeField] private TutorialWindowPresenter m_tutorialWindowPrefab;

        private readonly List<IInstaller> m_installers = new List<IInstaller>();

        public async void Awake()
        {
            await BootScene();
        }

        public async Task BootScene()
        {
            InputInstaller inputInstaller = new InputInstaller();
            InputContext inputContext = inputInstaller.Install(m_mouseListenerPrefab);
            m_installers.Add(inputInstaller);

            if (m_seed == -1)
                m_seed = Environment.TickCount;

            System.Random random = new System.Random(m_seed);
            m_debugUI.Initialize(inputContext.UserInput, m_seed);
            m_escapeScreen.Initialize(inputContext.UserInput);

            WorldGrid grid = new WorldGrid(m_dungeonGrid, m_dungeonTilemap);
            GameWorld world = new GameWorld(grid, m_locations, random);
            m_worldPresenter.Initialize(world);

            SpawnServiceInstaller spawnServiceInstaller = new SpawnServiceInstaller();
            SpawnService spawner = spawnServiceInstaller.Install(m_entityPrefab, m_projectilePrefab, grid);
            m_installers.Add(spawnServiceInstaller);

            EffectDispatcher effectDispatcher = new EffectDispatcher();
            effectDispatcher = new EffectDispatcher();
            effectDispatcher.Register(new SpendResourceHandler());
            effectDispatcher.Register(new DamageHandler());
            effectDispatcher.Register(new MoveHandler());
            effectDispatcher.Register(new RestoreParameterHandler());
            effectDispatcher.Register(new HealHandler());
            effectDispatcher.Register(new DecreaseCooldownHandler());
            effectDispatcher.Register(new KnockbackHandler());
            effectDispatcher.Register(new TeleportHandler());
            AbilityExecutionRunner abilityExecutionRunner = new AbilityExecutionRunner(m_scriptDatabase, effectDispatcher, world);
            IStartOfTurnEffectFactory startOfTurnEffectFactory = new StartOfTurnEffectFactory(m_parameterDatabase.Energy, m_energyRegenPerTurn);

            PlayerInstaller playerInstaller = new PlayerInstaller();
            PlayerContext playerContext = playerInstaller.Install(spawner.EntitySpawner, m_playerData, inputContext, abilityExecutionRunner, world, m_graphicRaycaster);
            m_installers.Add(playerInstaller);

            AIInstaller aiInstaller = new AIInstaller();
            AIContext aiContext = aiInstaller.Install(m_strategy, startOfTurnEffectFactory, effectDispatcher, m_lookAhead);
            m_installers.Add(aiInstaller);

            ServiceInstaller serviceInstaller = new ServiceInstaller();
            MagmaHeartServices services = serviceInstaller.Install(spawner);
            m_installers.Add(serviceInstaller);

            BattleInstaller battleInstaller = new BattleInstaller();
            BattleContext battleContext = battleInstaller.Install(services.SpawnService.EntitySpawner, aiContext, random, m_minDistanceFromPlayer, world, playerContext.TurnController, abilityExecutionRunner, effectDispatcher, startOfTurnEffectFactory);
            m_installers.Add(battleInstaller);

            CameraController camera = Instantiate(m_cameraPrefab, new Vector3(0, 0, -10), Quaternion.identity);
            camera.Initialize(playerContext.Player.transform, inputContext.UserInput, battleContext.Battle);

            StatisticsInstaller statisticsInstaller = new StatisticsInstaller();
            var counters = statisticsInstaller.Install(world);
            m_installers.Add(statisticsInstaller);

            Inventory inventory = new Inventory(playerContext.Player.Model);
            RewardService rewardService = new RewardService(inventory, m_artifactDatabase);

            m_gameUI.Initialize(playerContext.Player, battleContext.Battle, playerContext.TurnController, world, counters.roomCounter, counters.bossCounter, inventory);
            m_abilitySelectorPresenter.Initialize(world, playerContext.Player.Model, playerContext.TurnController, battleContext.Battle);

            TutorialInstaller tutorialInstaller = new TutorialInstaller();
            TutorialContext tutorialContext = tutorialInstaller.Install(m_windowDatabase, m_tutorialWindowPrefab, m_gameUI.transform);
            m_installers.Add(tutorialInstaller);

            MagmaHeartContext magmaHeartContext = new MagmaHeartContext(world, m_worldPresenter, playerContext.Player, services, camera, battleContext, m_gameUI, rewardService, tutorialContext);
            MagmaHeartStateMachine stateMachine = new MagmaHeartStateMachine(magmaHeartContext);
            
            await stateMachine.Start();
        }

        public void OnDisable()
        {
            foreach (IInstaller installer in m_installers)
                installer.Dispose();

            m_debugUI.Disable();
        }

        // This is only need for the tutorial and statistics
        // TODO: remove this after save system implementation
        private void OnApplicationQuit()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}