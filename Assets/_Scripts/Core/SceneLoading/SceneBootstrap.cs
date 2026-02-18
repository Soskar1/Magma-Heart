using MagmaHeart.Core.AI;
using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.BoardStateSystem.Actions.Preview;
using MagmaHeart.Core.CameraControls;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
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

        [Header("Input")]
        [SerializeField] private MouseListener m_mouseListenerPrefab;

        [Header("Actions")]
        [SerializeField] private int m_movementSpeed;

        [Header("Dungeon")]
        [SerializeField] private List<LocationData> m_locations;
        [SerializeField] private CombatTilemapRenderer m_combatTilemapRenderer;
        [SerializeField] private Tilemap m_dungeonTilemap;
        [SerializeField] private Grid m_dungeonGrid;
        [SerializeField] private RoomRenderer m_roomRenderer;
        [SerializeField] private int m_seed;
        [SerializeField] private TileBase m_floorTile;
        [SerializeField] private TileBase m_wallTile;

        [Header("Spawner Settings")]
        [SerializeField] private Entity m_entityPrefab;
        [SerializeField] private Projectile m_projectilePrefab;
        [SerializeField] private float m_minDistanceFromPlayer;

        [Header("UI")]
        [SerializeField] private MagmaHeartWindowDatabaseDefinition m_windowDatabase;
        [SerializeField] private GameUI m_gameUI;
        [SerializeField] private DebugUI m_debugUI;
        [SerializeField] private EscapeScreen m_escapeScreen;
        [SerializeField] private GraphicRaycaster m_graphicRaycaster;

        [Header("Tutorial")]
        [SerializeField] private TutorialWindowPresenter m_tutorialWindowPrefab;

        private HoverModeController m_hoverModeController;
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

            RoomGrid grid = new RoomGrid(m_dungeonGrid, m_dungeonTilemap);
            DungeonController dungeonController = new DungeonController(grid, m_locations, random);
            m_roomRenderer.Initialize(dungeonController);

            AIInstaller aiInstaller = new AIInstaller();
            AIContext aiContext = aiInstaller.Install();
            m_installers.Add(aiInstaller);

            SpawnServiceInstaller spawnServiceInstaller = new SpawnServiceInstaller();
            SpawnService spawner = spawnServiceInstaller.Install(m_entityPrefab, m_projectilePrefab, grid);
            m_installers.Add(spawnServiceInstaller);

            ServiceInstaller serviceInstaller = new ServiceInstaller();
            MagmaHeartServices services = serviceInstaller.Install(spawner);
            m_installers.Add(serviceInstaller);

            ActionRunnerInstaller actionRunnerInstaller = new ActionRunnerInstaller();
            ActionExecutor actionRunner = actionRunnerInstaller.Install(m_movementSpeed, spawner);
            m_installers.Add(actionRunnerInstaller);

            BattleInstaller battleInstaller = new BattleInstaller();
            BattleContext battleContext = battleInstaller.Install(services, aiContext, random, grid, m_minDistanceFromPlayer, dungeonController, actionRunner);
            m_installers.Add(battleInstaller);

            ActionPreviewInstaller actionPreviewInstaller = new ActionPreviewInstaller(m_combatTilemapRenderer);
            IActionPreviewProvider previewProvider = actionPreviewInstaller.Install(aiContext.ActionDatabase, battleContext.Battle, dungeonController);
            m_installers.Add(actionPreviewInstaller);

            PlayerInstaller playerInstaller = new PlayerInstaller();
            Entity player = playerInstaller.Install(spawner.EntitySpawner, m_playerData, inputContext, previewProvider, actionRunner);
            m_installers.Add(playerInstaller);

            CameraController camera = Instantiate(m_cameraPrefab, new Vector3(0, 0, -10), Quaternion.identity);
            camera.Initialize(player.transform, inputContext.UserInput, battleContext.Battle);

            m_hoverModeController = new HoverModeController(inputContext.MouseHoverEngine, dungeonController, m_graphicRaycaster, previewProvider, m_combatTilemapRenderer);
            m_hoverModeController.UseRaycastHover();

            StatisticsInstaller statisticsInstaller = new StatisticsInstaller();
            CompletedRoomsCounter completedRoomsCounter = statisticsInstaller.Install(dungeonController);
            m_installers.Add(statisticsInstaller);

            m_gameUI.Initialize(player, battleContext.Battle, inputContext.MouseHoverEngine, previewProvider, completedRoomsCounter);

            ArtifactInstaller artifactInstaller = new ArtifactInstaller();
            RewardService rewardService = artifactInstaller.Install(player.Model, m_gameUI.RewardUI);
            m_installers.Add(artifactInstaller);

            TutorialInstaller tutorialInstaller = new TutorialInstaller();
            TutorialContext tutorialContext = tutorialInstaller.Install(m_windowDatabase, m_tutorialWindowPrefab, m_gameUI.transform);
            m_installers.Add(tutorialInstaller);

            MagmaHeartContext magmaHeartContext = new MagmaHeartContext(dungeonController, m_roomRenderer, player, m_hoverModeController, services, camera, battleContext, m_gameUI, rewardService, tutorialContext);
            MagmaHeartStateMachine stateMachine = new MagmaHeartStateMachine(magmaHeartContext);
            
            await stateMachine.Start();
        }

        public void OnDisable()
        {
            foreach (IInstaller installer in m_installers)
                installer.Dispose();

            m_hoverModeController.Disable();

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