using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Pathfinding;
using MagmaHeart.AI.States;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Properties;
using MagmaHeart.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class MovementAction : MagmaHeart.AI.Actions.Action<MovementActionArgs>
    {
        private readonly TurnBasedMovement m_movement;
        private readonly Entity m_entity;
        private readonly Energy m_energy;
        private readonly AStar m_aStar;
        
        private Room m_currentRoom;
        private List<RoomTile> m_currentPath;
        private int m_freeDistanceToMove;
        private int m_currentTheoreticalFreeDistanceToMove;

        public int CurrentTheoreticalEnergyUsage { get; private set; } = 0;
        public int MovementDistanceInTilesForOneEnergy { get; private set; } = 2;
        public List<RoomTile> CurrentPath
        {
            get
            {
                if (m_currentPath == null)
                    m_currentPath = new List<RoomTile>();

                return m_currentPath;
            }
            private set
            {
                if (value == null)
                    return;

                m_currentPath = value;
            }
        }

        public MovementAction(Entity actionPossessor) : base(actionPossessor.Model)
        {
            m_entity = actionPossessor;
            m_energy = m_entity.Model.Energy;
            m_movement = m_entity.TurnBasedMovement;
            m_aStar = new AStar(AStar.ManhattanDistance);
            CurrentPath = new List<RoomTile>();
        }

        public void Reset()
        {
            m_freeDistanceToMove = 0;
            CurrentTheoreticalEnergyUsage = 0;
            CurrentPath.Clear();
        }

        // TODO: Use IBattleStartedListener to set the current room
        public void SetCurrentRoom(Room room) => m_currentRoom = room;

        public override ActionArgs CreateActionArgs(StateSnapshot state, SimulatedBoard board, AIUnit unit)
        {
            //Vector3Int source = state.GetProperty<PositionPropertySnapshot>(ActionPossessor).Position;
            Vector3Int targetPosition = state.GetProperty<PositionPropertySnapshot>(unit).Position;

            //// Target tile is an obstacle. Need to get closest (to the possessor) adjacent tile
            //Vector3Int[] adjacentTiles = new Vector3Int[]
            //{
            //    target + Vector3Int.up,
            //    target + Vector3Int.down,
            //    target + Vector3Int.left,
            //    target + Vector3Int.right
            //};

            //Vector3Int closest = adjacentTiles
            //    .OrderBy(tile => DungeonGrid.ManhattanDistance(tile, source))
            //    .First();

            //RoomTile roomTile = m_currentRoom.GetRoomTile(closest);

            RoomTile target = m_currentRoom.GetRoomTile(targetPosition);
            Func<RoomTile, bool> tileIsFree = (tile) =>
            {
                Vector2 tileVector2 = tile.Position.ToVector2();
                return board.IsBoardNodeEmpty(tileVector2);
            };
            target = PickAdjacentFreeTile(target, tileIsFree);

            return new MovementActionArgs(target);
        }

        public override bool CanSimulate(StateSnapshot state, SimulatedBoard room, MovementActionArgs args)
        {
            PositionPropertySnapshot possessorPosition = state.GetProperty<PositionPropertySnapshot>(ActionPossessor);

            float distanceToTarget = DungeonGrid.ManhattanDistance(possessorPosition.Position, args.TileToMove.Position);
            Debug.Log($"[{state.CurrentSimulationDepth}-MovementAction-{args.TileToMove.Position}-({m_entity.gameObject.name})] Manhattan distance to target: {distanceToTarget}");

            if (distanceToTarget <= 1)
            {
                Debug.Log($"[{state.CurrentSimulationDepth}-MovementAction-{args.TileToMove.Position}-({m_entity.gameObject.name})] In this simulation entity is too close to the target!");
                return false;
            }

            List<Vector2> path = m_aStar.FindPath(room.Graph, possessorPosition.Position.ToVector2(), args.TileToMove.Position.ToVector2());

            if (!path.Any())
            {
                Debug.Log($"[{state.CurrentSimulationDepth}-MovementAction-{args.TileToMove.Position}-({m_entity.gameObject.name})] In this simulation path to the target does not exist! (From {possessorPosition.Position}, to {args.TileToMove.Position})");
                return false;
            }

            Debug.Log($"[{state.CurrentSimulationDepth}-MovementAction-{args.TileToMove.Position}-({m_entity.gameObject.name})] It is possible to simulate MovementAction");
            return true;
        }

        public override bool CanSimulate(StateSnapshot state, SimulatedBoard board, ActionArgs args) => CanSimulate(state, board, args as MovementActionArgs);

        public override StateSnapshot Simulate(StateSnapshot state, SimulatedBoard room, MovementActionArgs args)
        {
            StateSnapshot newState = base.Simulate(state, room, args);

            PositionPropertySnapshot possessorPosition = state.GetProperty<PositionPropertySnapshot>(ActionPossessor);

            // TODO: Is it possible to not culate the whole path again?
            List<RoomTile> path = m_aStar.FindPath(room.Graph, possessorPosition.Position.ToVector2(), args.TileToMove.Position.ToVector2())
                .Select(v => m_currentRoom.GetRoomTile(v))
                .ToList();

            EnergyPropertySnapshot possessorEnergy = newState.GetProperty<EnergyPropertySnapshot>(ActionPossessor);
            int distanceToMove = (possessorEnergy.CurrentEnergy + 5) * MovementDistanceInTilesForOneEnergy;
            distanceToMove = Math.Min(distanceToMove, path.Count) - 1;
            RoomTile currentMovementTarget = path[distanceToMove];

            // TODO: Calculate free movement. Save it as a property.

            PositionPropertySnapshot newPosition = new PositionPropertySnapshot(currentMovementTarget.Position);
            newState.Update(ActionPossessor, newPosition);

            NodeTypeBoardModification sourceNodeModification = new NodeTypeBoardModification(path.First().Position.ToVector2(), BoardNodeType.Walkable);
            NodeTypeBoardModification targetNodeModification = new NodeTypeBoardModification(currentMovementTarget.Position.ToVector2(), BoardNodeType.Obstacle);
            room.ApplyBoardModification(newState.CurrentSimulationDepth, sourceNodeModification);
            room.ApplyBoardModification(newState.CurrentSimulationDepth, targetNodeModification);

            return newState;
        }

        public override void Execute(MovementActionArgs args)
        {
            CalculatePath(args.TileToMove);

            int distanceToMove = m_entity.Model.Energy.CurrentEnergy * MovementDistanceInTilesForOneEnergy;
            distanceToMove = Math.Min(distanceToMove, CurrentPath.Count) - 1 - m_freeDistanceToMove;
            RoomTile currentMovementTarget = CurrentPath[distanceToMove];

            int energyUsage = Mathf.CeilToInt(distanceToMove / (float)MovementDistanceInTilesForOneEnergy);
            m_freeDistanceToMove = distanceToMove % MovementDistanceInTilesForOneEnergy;
            m_energy.Spend(energyUsage);

            Move();
        }

        public override void Execute(ActionArgs args) => Execute(args as MovementActionArgs);

        public bool CanExecute(RoomTile targetTile)
        {
            if (m_entity.Energy.CurrentEnergy <= 0)
                return false;

            CalculatePath(targetTile);

            int distance = CurrentPath.Count - 1 - m_freeDistanceToMove;
            CurrentTheoreticalEnergyUsage = Mathf.CeilToInt(distance / (float)MovementDistanceInTilesForOneEnergy);
            m_currentTheoreticalFreeDistanceToMove = distance % MovementDistanceInTilesForOneEnergy;

            return m_energy.HasEnough(CurrentTheoreticalEnergyUsage);
        }

        private void Move()
        {
            if (CurrentPath.Count <= 0)
            {
                Debug.Log("Path is empty. Can't move.");
                return;
            }

            m_movement.StartMovement(CurrentPath);
        }

        public void MoveWithoutEnergyUsage(RoomTile targetTile)
        {
            CalculatePath(targetTile);
            Move();
        }

        private void CalculatePath(RoomTile targetTile)
        {
            // TODO: implement cache

            if (!m_currentRoom.TileIsAccessable(targetTile))
                targetTile = PickAdjacentFreeTile(targetTile, m_currentRoom.TileIsAccessable);

            Vector3Int currentTile = m_entity.Model.GetCurrentTilePosition();
            List<Vector2> path = m_aStar.FindPath(m_currentRoom.Graph, currentTile.ToVector2(), targetTile.Position.ToVector2());

            if (path != null && path.Count > 0)
                CurrentPath = path.Select(v => m_currentRoom.GetRoomTile(v)).ToList();
        }

        private RoomTile PickAdjacentFreeTile(RoomTile targetTile, Func<RoomTile, bool> tileIsAccessable)
        {
            RoomTile tile = targetTile;
            Vector3Int[] adjacentTiles = new Vector3Int[]
            {
                targetTile.Position + Vector3Int.up,
                targetTile.Position + Vector3Int.down,
                targetTile.Position + Vector3Int.left,
                targetTile.Position + Vector3Int.right
            };

            List<RoomTile> roomTiles =
                adjacentTiles.OrderBy(tile => DungeonGrid.ManhattanDistance(tile, m_entity.Model.GetCurrentTilePosition()))
                .Select(v => m_currentRoom.GetRoomTile(v))
                .ToList();

            foreach (RoomTile roomTile in roomTiles)
            {
                if (tileIsAccessable(roomTile))
                {
                    tile = roomTile;
                    break;
                }
            }

            return tile;
        }
    }
}