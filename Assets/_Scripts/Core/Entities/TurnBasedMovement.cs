using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class TurnBasedMovement : MonoBehaviour
    {
        [SerializeField] private int m_speed = 5;
        [SerializeField] private float m_changePointAtDistance = 0.001f;
        private int m_targetIndex;
        private bool m_canMove = false;
        private List<Vector2> m_currentPath = new List<Vector2>();

        public event EventHandler OnMovementStarted;
        public event EventHandler OnMovementEnded;

        public void StartMovement(List<Vector2> path)
        {
            m_currentPath = path;
            m_targetIndex = 0;
            m_canMove = true;
            OnMovementStarted?.Invoke(this, EventArgs.Empty);
        }

        public void Update()
        {
            if (!m_canMove)
                return;

            Vector2 target = m_currentPath[m_targetIndex];

            transform.position = Vector3.MoveTowards(transform.position, target, m_speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < m_changePointAtDistance)
            {
                transform.position = target;
                ++m_targetIndex;
            }

            if (m_targetIndex == m_currentPath.Count)
            {
                m_canMove = false;
                OnMovementEnded?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}