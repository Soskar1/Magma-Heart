using MagmaHeart.DungeonGeneration;
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
        private List<DungeonTile> m_currentPath = new List<DungeonTile>();

        private TaskCompletionSource<bool> m_movementFinished;

        public Task StartMovementAsync(List<DungeonTile> path, int speed)
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

            DungeonTile target = m_currentPath[m_targetIndex];

            transform.position = Vector3.MoveTowards(transform.position, target.Position.ToVector3(), m_speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.Position.ToVector3()) < m_changePointAtDistance)
            {
                transform.position = target.Position.ToVector3();
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