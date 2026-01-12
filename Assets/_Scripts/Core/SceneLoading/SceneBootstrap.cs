using Magmaheart.Core.Tutorial;
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
using MagmaHeart.UIWindowPopupSystem;
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
        [SerializeField] private WindowPresenter m_tutorialWindowPrefab;
        [SerializeField] private MagmaHeartWindowDatabaseDefinition m_windowDatabase;
        [SerializeField] private GameUI m_gameUI;
        [SerializeField] private DebugUI m_debugUI;
        [SerializeField] private GraphicRaycaster m_graphicRaycaster;

        [Header("Travel")]
        [SerializeField] private int m_travelSpeed;

        private HoverModeController m_hoverModeController;

        private InputInstaller m_inputInstaller;
        private AIInstaller m_aiInstaller;
        private PlayerInstaller m_playerInstaller;
        private ActionPreviewInstaller m_actionPreviewInstaller;
        private BattleInstaller m_battleInstaller;
        private SpawnServiceInstaller m_spawnServiceInstaller;
        private ServiceInstaller m_serviceInstaller;
        private ArtifactInstaller m_artifactInstaller;

        public async void Awake()
        {
            await BootScene();
        }

        public async Task BootScene()
        {
            m_inputInstaller = new InputInstaller();
            InputContext inputContext = m_inputInstaller.Install(m_mouseListenerPrefab);

            if (m_seed == -1)
                m_seed = Environment.TickCount;

            System.Random random = new System.Random(m_seed);
            m_debugUI.Initialize(inputContext.UserInput, m_seed);

            RoomGrid grid = new RoomGrid(m_dungeonGrid, m_dungeonTilemap);
            DungeonController dungeonController = new DungeonController(grid, m_locations, random);
            m_roomRenderer.Initialize(dungeonController);

            m_aiInstaller = new AIInstaller();
            AIContext aiContext = m_aiInstaller.Install();

            m_spawnServiceInstaller = new SpawnServiceInstaller();
            SpawnService spawner = m_spawnServiceInstaller.Install(m_entityPrefab, m_projectilePrefab, grid);
            
            m_serviceInstaller = new ServiceInstaller();
            MagmaHeartServices services = m_serviceInstaller.Install(spawner);

            m_battleInstaller = new BattleInstaller();
            BattleContext battleContext = m_battleInstaller.Install(services, aiContext, random, grid, m_minDistanceFromPlayer);

            m_actionPreviewInstaller = new ActionPreviewInstaller(m_combatTilemapRenderer);
            IActionPreviewProvider previewProvider = m_actionPreviewInstaller.Install(aiContext.ActionDatabase, battleContext.Battle, dungeonController);

            m_playerInstaller = new PlayerInstaller();
            Entity player = m_playerInstaller.Install(spawner.EntitySpawner, m_playerData, inputContext, previewProvider);

            CameraController camera = Instantiate(m_cameraPrefab, new Vector3(0, 0, -10), Quaternion.identity);
            camera.Initialize(player.transform, inputContext.UserInput, battleContext.Battle);

            m_hoverModeController = new HoverModeController(inputContext.MouseHoverEngine, dungeonController, m_graphicRaycaster, previewProvider, m_combatTilemapRenderer);
            m_hoverModeController.UseRaycastHover();

            m_gameUI.Initialize(player, battleContext.Battle, inputContext.MouseHoverEngine, previewProvider);
            m_artifactInstaller = new ArtifactInstaller();
            RewardService rewardService = m_artifactInstaller.Install(player.Model, m_gameUI.RewardUI);

            MagmaHeartContext magmaHeartContext = new MagmaHeartContext(dungeonController, m_roomRenderer, player, m_hoverModeController, services, camera, battleContext, m_gameUI, rewardService, new Tutorial());
            MagmaHeartStateMachine stateMachine = new MagmaHeartStateMachine(magmaHeartContext);
            
            await stateMachine.Start();
        }

        public void OnDisable()
        {
            m_inputInstaller.Dispose();
            m_aiInstaller.Dispose();
            m_playerInstaller.Dispose();
            m_spawnServiceInstaller.Dispose();
            m_actionPreviewInstaller.Dispose();
            m_battleInstaller.Dispose();
            m_serviceInstaller.Dispose();
            m_artifactInstaller.Dispose();

            m_hoverModeController.Disable();

            m_debugUI.Disable();
        }

        // This is only need for the tutorial
        // TODO: remove this after save system implementation
        private void OnApplicationQuit()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}