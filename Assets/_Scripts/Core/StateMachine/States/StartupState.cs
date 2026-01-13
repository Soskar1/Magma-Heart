using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.SceneLoading;
using MagmaHeart.Core.TutorialSystem;
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
        private readonly TutorialContext m_tutorial;

        public StartupState(MagmaHeartStateMachine stateMachine, MagmaHeartContext context)
        {
            m_context = context;
            m_stateMachine = stateMachine;

            m_tutorial = m_context.Tutorial;
        }

        public async Task EnterAsync(StatePayload payload = null)
        {
            Entity player = m_context.Player;
            DungeonController dungeon = m_context.DungeonController;

            RoomModel roomModel = dungeon.GenerateRoom();
            await m_context.RoomRenderer.OnRoomRendered;
            
            if (!m_tutorial.Model.IsSet(TutorialFlags.OpenedWelcomeScreen))
            {
                m_context.UI.WelcomeScreen.Open();
                m_tutorial.Model.TrySetFlag(TutorialFlags.OpenedWelcomeScreen);
            }

            Vector2 center = dungeon.Grid.ToTileCenter(roomModel.WorldPosition);
            m_context.CameraController.MoveTo(center);
            player.transform.position = center;
           
            player.gameObject.SetActive(true);

            m_context.HoverModeController.UseRaycastHover();

            await m_context.UI.WelcomeScreen.GetTask();

            m_tutorial.Model.TrySetFlag(TutorialFlags.HealthBarExplained);

            await m_tutorial.Presenter.GetUntilWindowCloseTask(TutorialFlags.HealthBarExplained);

            TravelStatePayload travelPayload = new TravelStatePayload(TravelReason.ExitRoom);
            await m_stateMachine.FireTrigger(StateMachineTriggers.StartupComplete, travelPayload);
        }

        public Task ExitAsync()
        {
            return Task.CompletedTask;
        }
    }
}
