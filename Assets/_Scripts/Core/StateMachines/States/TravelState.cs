using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.SceneLoading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagmaHeart.Core.StateMachines
{
    public class TravelState : IState
    {
        private readonly MagmaHeartStateMachine m_stateMachine;
        private readonly MagmaHeartContext m_context;
        private readonly int m_travelSpeed;

        public TravelState(MagmaHeartStateMachine stateMachine, MagmaHeartContext context, int travelSpeed)
        {
            m_stateMachine = stateMachine;
            m_context = context;
            m_travelSpeed = travelSpeed;
        }

        public async Task EnterAsync()
        {
            m_context.DungeonController.GenerateRoom();

            Player player = m_context.Player;
            Room room = m_context.DungeonController.CurrentRoom;

            m_context.CameraController.MoveTo(room.RoomModel.WorldPosition);
            m_context.HoverModeController.UseRaycastHover();

            player.transform.position = room.RoomModel.EntranceDoor.Position.ToVector3();

            RoomTile start = room.GetRoomTile(player.transform.position);
            RoomTile end = room.GetRoomTile(room.RoomModel.WorldPosition.ToVector3Int());
            List<RoomTile> path = new List<RoomTile>() { start, end };
            
            await m_context.EntityMovementService.MoveEntityAsync(player, path, m_travelSpeed);

            await m_stateMachine.FireTrigger(StateMachineTriggers.BattleStarted);
        }

        public Task ExitAsync()
        {
            return Task.CompletedTask;
        }
    }
}