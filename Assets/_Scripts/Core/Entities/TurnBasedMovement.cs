using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class TurnBasedMovement : MonoBehaviour
    {
        [SerializeField] private int m_speed = 5;
        [SerializeField] private float m_changePointAtDistance = 0.001f;
        private int m_targetIndex;
        private bool m_canMove = false;
        private List<RoomTile> m_currentPath = new List<RoomTile>();

        public event EventHandler<OnMovementEventArgs> OnMovementStarted;
        public event EventHandler<OnMovementEventArgs> OnMovementEnded;
        private OnMovementEventArgs m_currentArgs;

        public void StartMovement(List<RoomTile> path)
        {
            m_currentPath = path;
            m_targetIndex = 0;
            m_canMove = true;

            m_currentArgs = new OnMovementEventArgs(path.First().Position, path.Last().Position);
            OnMovementStarted?.Invoke(this, m_currentArgs);
        }

        public void Update()
        {
            if (!m_canMove)
                return;

            RoomTile target = m_currentPath[m_targetIndex];

            transform.position = Vector3.MoveTowards(transform.position, target.TileCenter, m_speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.TileCenter) < m_changePointAtDistance)
            {
                transform.position = target.TileCenter;
                ++m_targetIndex;
            }

            if (m_targetIndex == m_currentPath.Count)
            {
                m_canMove = false;
                OnMovementEnded?.Invoke(this, m_currentArgs);
            }
        }
    }
}