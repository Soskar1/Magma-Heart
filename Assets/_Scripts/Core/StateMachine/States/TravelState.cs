using MagmaHeart.AI.Pathfinding;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.SceneLoading;
using MagmaHeart.Core.StateMachine.States;
using MagmaHeart.DungeonGeneration;
using MagmaHeart.StateMachine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly AStar m_aStar = new AStar(AStar.ManhattanDistance);

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

            List<Vector3> aStarPath = new List<Vector3>();
            if (isEnteringRoom)
            {
                m_context.CameraController.MoveTo(room.RoomModel.WorldPosition);

                var bestPath = new List<Vector2>();
                foreach (var adjacentTile in room.RoomModel.GetAdjacentTiles(room.RoomModel.EntranceDoor.Position))
                {
                    var tmpPath = m_aStar.FindPath(room.Graph, adjacentTile.Position, room.RoomModel.WorldPosition);

                    if (tmpPath == null)
                        continue;

                    if (tmpPath.Count < bestPath.Count || bestPath.Count == 0)
                        bestPath = tmpPath;
                }

                Vector2 startTile = m_world.ToTileCenter(room.RoomModel.EntranceDoor.Position);
                aStarPath.Add(startTile);
                aStarPath.AddRange(bestPath.Select(tile => (Vector3)tile));
                aStarPath.Add(m_world.ToTileCenter(room.RoomModel.WorldPosition));
            }
            else
            {
                var startTile = m_world.WorldToTilePosition(player.transform.position.ToVector2Int());
                var bestPath = new List<Vector2>();
                foreach (var adjacentTile in room.RoomModel.GetAdjacentTiles(room.RoomModel.ExitDoor.Position))
                {
                    var tmpPath = m_aStar.FindPath(room.Graph, startTile, adjacentTile.Position);

                    if (tmpPath == null)
                        continue;

                    if (tmpPath.Count < bestPath.Count || bestPath.Count == 0)
                        bestPath = tmpPath;
                }

                aStarPath.AddRange(bestPath.Select(tile => (Vector3)tile));
                var endTile = m_world.ToTileCenter(room.RoomModel.ExitDoor.Position);
                aStarPath.Add(endTile);
            }

            for (int i = 0; i < aStarPath.Count - 1; i++)
                aStarPath[i] = m_world.ToTileCenter(aStarPath[i].ToVector2Int());
            
            player.transform.position = aStarPath[0];

            await m_context.Services.MovementService.MoveEntityAsync(player, aStarPath, m_travelSpeed);
            
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