using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.SceneLoading;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.StateMachines
{
    public class TravelState : IState
    {
        private readonly MagmaHeartStateMachine m_stateMachine;
        private readonly MagmaHeartContext m_context;

        public TravelState(MagmaHeartStateMachine stateMachine, MagmaHeartContext context)
        {
            m_stateMachine = stateMachine;
            m_context = context;
        }

        public async Task EnterAsync()
        {
            Debug.Log("Enter Travel state");

            m_context.DungeonController.GenerateRoom();

            Player player = m_context.Player;
            Room room = m_context.DungeonController.CurrentRoom;

            m_context.HoverModeController.UseRaycastHover();
            player.transform.position = new Vector2(room.RoomModel.LeftBorder, room.RoomModel.WorldPosition.y);

            RoomTile start = room.GetRoomTile(player.transform.position);
            RoomTile end = room.GetRoomTile(room.RoomModel.WorldPosition.ToVector3Int());
            List<RoomTile> path = new List<RoomTile>() { start, end };
            
            await m_context.EntityMovementService.MoveEntityAsync(player, path);

            await m_stateMachine.FireTrigger(StateMachineTriggers.BattleStarted);
        }

        public Task ExitAsync()
        {
            Debug.Log("Exit Travel state");
            return Task.CompletedTask;
        }
    }
}