using System;
using System.Collections;
using MagmaHeart.Core.CombatSystem;
using UnityEngine;

namespace MagmaHeart.Core.Entities.NonPlayableCharacters
{
    public class Enemy : MonoBehaviour, IEntity, ITurnController
    {
        [SerializeField] private float m_maxHealth;

        private Entity m_controllingEntity;
        public Entity ControllingEntity => m_controllingEntity;

        public Action NextTurn { get; set; }

        public void Initialize()
        {
            m_controllingEntity = new Entity(m_maxHealth);
        }

        public void StartTurn()
        {
            Debug.Log($"{gameObject.name} ({gameObject.transform.position}) is doing a move");
            StartCoroutine(MakingThinkingMove());
        }

        public void EndTurn()
        {
            Debug.Log($"{gameObject.name} ({gameObject.transform.position}) ended his move");
            NextTurn?.Invoke();
        }

        private IEnumerator MakingThinkingMove()
        {
            yield return new WaitForSeconds(4);
            EndTurn();
        }
    }
}