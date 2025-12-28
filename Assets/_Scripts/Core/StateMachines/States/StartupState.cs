using MagmaHeart.Core.AI;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.SceneLoading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.StateMachines.States
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

        public async Task EnterAsync()
        {
            Debug.Log("Entered startup state");

            Player player = m_context.Player;
            DungeonController dungeon = m_context.DungeonController;

            dungeon.GenerateRoom();
            player.gameObject.SetActive(true);
            player.transform.position = new Vector2(dungeon.CurrentRoom.RoomModel.LeftBorder, dungeon.CurrentRoom.RoomModel.WorldPosition.y);

            await m_stateMachine.FireTrigger(StateMachineTriggers.StartupComplete);
        }

        public Task ExitAsync()
        {
            Debug.Log("Exit startup state");
            return Task.CompletedTask;
        }
    }
}
