using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.SceneLoading;
using MagmaHeart.Core.StateMachine.States;
using MagmaHeart.StateMachine;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.StateMachine
{
    public class TravelState : IState
    {
        private readonly MagmaHeartStateMachine m_stateMachine;
        private readonly MagmaHeartContext m_context;
        private const int m_travelSpeed = TileBasedMovement.DEFAULT_SPEED * 2;

        private readonly GameWorld m_world;

        public TravelState(MagmaHeartStateMachine stateMachine, MagmaHeartContext context)
        {
            m_stateMachine = stateMachine;
            m_context = context;

            m_world = m_context.World;
        }

        public async Task EnterAsync(StatePayload payload)
        {
            TravelStatePayload travelPayload = payload as TravelStatePayload;

            Entity player = m_context.Player;
            Room room = m_world.CurrentRoom;

            bool isEnteringRoom = travelPayload.Reason == TravelReason.EnterRoom;

            Vector2 startPosition = m_world.ToTileCenter(isEnteringRoom ? room.RoomModel.EntranceDoor.Position : player.transform.position.ToVector2Int());
            Vector2 endPosition = m_world.ToTileCenter(isEnteringRoom ? room.RoomModel.WorldPosition : room.RoomModel.ExitDoor.Position);

            if (isEnteringRoom)
                m_context.CameraController.MoveTo(endPosition);

            player.transform.position = startPosition;

            await m_context.Services.MovementService.MoveEntityAsync(player, new List<Vector3> { startPosition, endPosition });
            
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