using MagmaHeart.Core.Dungeon;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        private TaskCompletionSource<bool> m_movementFinished;

        public Task StartMovementAsync(List<RoomTile> path)
        {
            m_currentPath = path;
            m_targetIndex = 0;
            m_canMove = true;

            m_movementFinished = new TaskCompletionSource<bool>();
            return m_movementFinished.Task;
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
                m_movementFinished.SetResult(true);
            }
        }
    }
}