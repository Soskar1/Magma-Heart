using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class FieldOfView
    {
        private Transform m_pivot;
        
        private float m_radius;
        private float m_rotZ;
        private Vector2 m_currentRaycastDirection = Vector2.right;

        public FieldOfView(in float radius, in int amountOfRaycasts, in Transform pivot)
        {
            m_rotZ = 360.0f / amountOfRaycasts;
            m_radius = radius;
            m_pivot = pivot;
        }

        public List<Transform> FindEntitiesInFieldOfView()
        {
            List<Transform> entities = new List<Transform>();
            RaycastHit2D[] hits = Physics2D.RaycastAll(m_pivot.transform.position, m_currentRaycastDirection, m_radius);

            foreach (RaycastHit2D hit in hits)
                if (hit.collider.GetComponent<IHittable>() != null)
                    entities.Add(hit.transform);

            m_currentRaycastDirection = m_currentRaycastDirection.Rotate(m_rotZ);

            return entities;
        }
    }
}