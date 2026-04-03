using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    [System.Serializable]
    public class StarPoint
    {
        [SerializeField] private Transform m_point;
        [SerializeField] private int m_starSortingOrder;
        [SerializeField] private int m_trailSortingOrder;
    
        public Transform Point => m_point;
        public int StarSortingOrder => m_starSortingOrder;
        public int TrailSortingOrder => m_trailSortingOrder;
    }

    public class StarPathMover : MonoBehaviour
    {
        [SerializeField] private List<StarPoint> m_points;
        [SerializeField] private float m_speed = 2f;

        private int m_currentIndex = 0;
        private int m_nextIndex = 1;
        private float m_timer;

        private SpriteRenderer m_spriteRenderer;
        private TrailRenderer m_trailRenderer;

        void Awake()
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
            m_trailRenderer = GetComponent<TrailRenderer>();
        }

        void Update()
        {
            if (m_points.Count < 2) return;

            m_timer += Time.deltaTime * m_speed;

            transform.position = Vector3.Lerp(
                m_points[m_currentIndex].Point.position,
                m_points[m_nextIndex].Point.position,
                m_timer
            );

            if (m_timer >= 1f)
            {
                m_timer = 0f;

                m_currentIndex = m_nextIndex;
                m_nextIndex = (m_nextIndex + 1) % m_points.Count;

                ApplySorting(m_points[m_currentIndex]);
            }
        }

        void ApplySorting(StarPoint point)
        {
            m_spriteRenderer.sortingOrder = point.StarSortingOrder;
            m_trailRenderer.sortingOrder = point.TrailSortingOrder;
        }
    }
}