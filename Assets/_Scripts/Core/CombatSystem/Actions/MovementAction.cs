using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.AI.Pathfinding;
using MagmaHeart.Extensions;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using MagmaHeart.AI;
using MagmaHeart.Core.Entities.Properties;

namespace MagmaHeart.Core.CombatSystem
{
    public class MovementAction : MagmaHeart.AI.Action
    {
        private readonly TurnBasedMovement m_movement;
        private readonly Entity m_entity;
        private readonly Energy m_energy;
        private readonly AStar m_aStar;
        
        private Room m_currentRoom;
        private List<RoomTile> m_currentPath;
        private int m_freeDistanceToMove;
        private int m_currentTheoreticalFreeDistanceToMove;
        private bool m_isEnabled;

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
        public RoomTile TileToMove { get; set; }

        public MovementAction(Entity actionPossessor) : base(actionPossessor.Model)
        {
            m_entity = actionPossessor;
            m_energy = m_entity.Model.Energy;
            m_movement = m_entity.TurnBasedMovement;
            m_aStar = new AStar(AStar.ManhattanDistance);
            CurrentPath = new List<RoomTile>();
        }

        public void Enable()
        {
            if (m_isEnabled)
                return;

            m_isEnabled = true;
        }

        public void Disable()
        {
            m_isEnabled = false;
        }

        public void Reset()
        {
            m_freeDistanceToMove = 0;
            CurrentTheoreticalEnergyUsage = 0;
            CurrentPath.Clear();
        }

        public void SetCurrentRoom(Room room) => m_currentRoom = room;

        public override bool CanSimulate(StateSnapshot state, AIUnit target)
        {
            EnergyProperty possessorEnergy = state.GetProperty<EnergyProperty>(ActionPossessor);
            if (possessorEnergy.CurrentEnergy <= 0)
                return false;

            PositionProperty possessorPosition = state.GetProperty<PositionProperty>(ActionPossessor);
            PositionProperty targetPosition = state.GetProperty<PositionProperty>(target);

            if (possessorPosition.ManhattanDistance(targetPosition) <= 1)
                return false;

            return true;
        }

        public override StateSnapshot Simulate(StateSnapshot state, AIUnit target)
        {
            StateSnapshot newState = base.Simulate(state, target);



            return newState;
        }

        public override void Execute()
        {
            if (CanMoveToTile(TileToMove))
            {
                m_energy.Spend(CurrentTheoreticalEnergyUsage);
                Move();
                m_freeDistanceToMove = m_currentTheoreticalFreeDistanceToMove;
            }
        }

        public bool CanMoveToTile(RoomTile targetTile)
        {
            // TODO: implement cache

            if (!m_currentRoom.TileIsAccessable(targetTile))
                return false;

            CalculatePath(targetTile);

            int distance = CurrentPath.Count - 1 - m_freeDistanceToMove;
            CurrentTheoreticalEnergyUsage = Mathf.CeilToInt(distance / (float)MovementDistanceInTilesForOneEnergy);
            m_currentTheoreticalFreeDistanceToMove = distance % MovementDistanceInTilesForOneEnergy;

            return m_energy.HasEnough(CurrentTheoreticalEnergyUsage);
        }

        private void Move()
        {
            if (!m_isEnabled)
                throw new InvalidOperationException("Movement action is not enabled");

            if (CurrentPath.Count == 0)
                return;

            m_movement.StartMovement(CurrentPath);
        }

        public void MoveWithoutEnergyUsage(RoomTile targetTile)
        {
            CalculatePath(targetTile);
            Move();
        }

        private void CalculatePath(RoomTile targetTile)
        {
            Vector3Int currentTile = m_entity.CurrentTilePosition;
            List<Vector2> path = m_aStar.FindPath(m_currentRoom.Graph, currentTile.ToVector2(), targetTile.Position.ToVector2());
            CurrentPath = path.Select(v => m_currentRoom.GetRoomTile(v)).ToList();
        }
    }
}