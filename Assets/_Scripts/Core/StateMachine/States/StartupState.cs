using MagmaHeart.Core.AI;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.SceneLoading;
using MagmaHeart.DungeonGeneration;
using MagmaHeart.StateMachine;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.StateMachine.States
{
    public class StartupState : IState
    {
        private readonly MagmaHeartStateMachine m_stateMachine;
        private readonly MagmaHeartContext m_context;

        public StartupState(MagmaHeartStateMachine stateMachine, MagmaHeartContext context)
        {
            m_context = context;
            m_stateMachine = stateMachine;
        }

        public async Task EnterAsync(StatePayload payload = null)
        {
            Entity player = m_context.Player;
            DungeonController dungeon = m_context.DungeonController;

            RoomModel roomModel = dungeon.GenerateRoom();
            await m_context.RoomRenderer.OnRoomRendered;

            // Show welcome screen for the first time

            Vector2 center = dungeon.Grid.ToTileCenter(roomModel.WorldPosition);
            m_context.CameraController.MoveTo(center);
            player.transform.position = center;
           
            player.gameObject.SetActive(true);

            m_context.HoverModeController.UseRaycastHover();

            // Wait for player to close the welcome screen

            await Task.Delay(5000);

            TravelStatePayload travelPayload = new TravelStatePayload(TravelReason.ExitRoom);
            await m_stateMachine.FireTrigger(StateMachineTriggers.StartupComplete, travelPayload);
        }

        public Task ExitAsync()
        {
            return Task.CompletedTask;
        }
    }
}
