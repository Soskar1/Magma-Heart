using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class Skeleton : MonoBehaviour, IHittable
    {
        [Header("Health")]
        [SerializeField] private float m_maxHealth;
        private Health m_health;

        [Header("Field of view")]
        [SerializeField] private float m_fieldOfViewRadius;
        [SerializeField] private int m_amountOfRaycasts;
        private FieldOfView m_fieldOfView;

        private void Awake()
        {
            m_health = new Health(m_maxHealth);
            m_fieldOfView = new FieldOfView(m_fieldOfViewRadius, m_amountOfRaycasts, transform);
        }

        private void Start()
        {
            m_health.OnTakeDamage += () => Debug.Log("Took damage");
        }

        private void Update()
        {
            List<Transform> entities = m_fieldOfView.FindEntitiesInFieldOfView();
            if (entities.Count > 1)
                Debug.Log($"Found entity {entities[1].name}");
        }

        public void Hit(in float damage) => m_health.TakeDamage(damage);
    }
}