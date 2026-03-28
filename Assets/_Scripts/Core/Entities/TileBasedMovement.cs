using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class TileBasedMovement : MonoBehaviour
    {
        [SerializeField] private float m_changePointAtDistance = 0.001f;
        public const int DEFAULT_SPEED = 5;
        private int m_speed = 5;
        private int m_targetIndex;
        private bool m_canMove = false;
        private List<Vector3> m_currentPath = new List<Vector3>();

        private TaskCompletionSource<bool> m_movementFinished;

        public event EventHandler OnChangedTarget;

        public Task StartMovementAsync(List<Vector3> path, int speed)
        {
            m_currentPath = path;
            m_targetIndex = 0;
            m_canMove = true;
            m_speed = speed;

            m_movementFinished = new TaskCompletionSource<bool>();
            return m_movementFinished.Task;
        }

        public void Update()
        {
            if (!m_canMove)
                return;

            Vector3 target = m_currentPath[m_targetIndex];

            transform.position = Vector3.MoveTowards(transform.position, target, m_speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < m_changePointAtDistance)
            {
                transform.position = target;
                ++m_targetIndex;

                OnChangedTarget?.Invoke(this, EventArgs.Empty);
            }

            if (m_targetIndex == m_currentPath.Count)
            {
                m_canMove = false;
                m_movementFinished.SetResult(true);
            }
        }
    }
}