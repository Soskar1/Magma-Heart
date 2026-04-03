using MagmaHeart.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Entities.Presenters
{
    public class StunPresenter : MonoBehaviour
    {
        private StarPathMover[] m_stunStars;
        private AIUnitModel m_model;

        public void Initialize(AIUnitModel model)
        {
            m_model = model;

            m_model.OnShouldSkipTurnChanged += HandleShouldSkipTurnChanged;
            m_stunStars = GetComponentsInChildren<StarPathMover>(true);
        }

        private void OnDisable()
        {
            m_model.OnShouldSkipTurnChanged -= HandleShouldSkipTurnChanged;
        }

        private void HandleShouldSkipTurnChanged(object _, bool shouldSkipTurn)
        {
            if (shouldSkipTurn)
            {
                foreach (StarPathMover star in m_stunStars)
                    star.gameObject.SetActive(true);
            }
            else
            {
                foreach (StarPathMover star in m_stunStars)
                    star.gameObject.SetActive(false);
            }
        }
    }
}
