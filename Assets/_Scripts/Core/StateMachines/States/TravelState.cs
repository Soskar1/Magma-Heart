using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.SceneLoading;
using MagmaHeart.Core.StateMachines.States;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.StateMachines
{
    public class TravelState : IState
    {
        private readonly MagmaHeartStateMachine m_stateMachine;
        private readonly MagmaHeartContext m_context;
        private const int m_travelSpeed = TileBasedMovement.DEFAULT_SPEED * 2;

        public TravelState(MagmaHeartStateMachine stateMachine, MagmaHeartContext context)
        {
            m_stateMachine = stateMachine;
            m_context = context;
        }

        public async Task EnterAsync(StatePayload payload)
        {
            TravelStatePayload travelPayload = payload as TravelStatePayload;

            Player player = m_context.Player;
            Room room = m_context.DungeonController.CurrentRoom;

            bool isEnteringRoom = travelPayload.Reason == TravelReason.EnterRoom;

            Vector3 startPosition = isEnteringRoom ? room.RoomModel.EntranceDoor.Position.ToVector3() : player.transform.position;
            Vector3 endPosition = isEnteringRoom ? room.RoomModel.WorldPosition.ToVector3() : room.RoomModel.ExitDoor.Position.ToVector3();

            if (isEnteringRoom)
                m_context.CameraController.MoveTo(endPosition);

            player.transform.position = startPosition;

            RoomTile start = room.GetRoomTile(startPosition);
            RoomTile end = room.GetRoomTile(endPosition);
            List<RoomTile> path = new List<RoomTile>() { start, end };

            await m_context.EntityMovementService.MoveEntityAsync(player, path, m_travelSpeed);

            StateMachineTriggers trigger = StateMachineTriggers.TravelCompleted_Enter;
            if (!isEnteringRoom)
                trigger = StateMachineTriggers.TravelCompleted_Exit;

            await m_stateMachine.FireTrigger(trigger);
        }

        public Task ExitAsync()
        {
            return Task.CompletedTask;
        }
    }
}