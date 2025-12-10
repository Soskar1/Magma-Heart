using MagmaHeart.AI.States;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Presenters;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem.Presenters
{
    public class TurnOrderPresenter : MonoBehaviour
    {
        [SerializeField] private EntityPresenter m_entityPresenterPrefab;
        private Dictionary<EntityModel, EntityPresenter> m_currentPresenters = new Dictionary<EntityModel, EntityPresenter>();

        private Battle m_battle;

        public void Initialize(Battle battle)
        {
            m_battle = battle;
            m_battle.OnBattleStarted += HandleOnBattleStarted;
            m_battle.OnEntityDied += HandleOnEntityDied;
            m_battle.OnBattleEnded += HandleOnBattleEnded;
        }

        private void OnDisable()
        {
            m_battle.OnBattleStarted -= HandleOnBattleStarted;
            m_battle.OnEntityDied -= HandleOnEntityDied;
            m_battle.OnBattleEnded -= HandleOnBattleEnded;
        }

        private void HandleOnBattleStarted(object obj, OnBattleStartedEventArgs args)
        {
            foreach (TurnContext<EntityModel> turnContext in args.TurnOrder)
            {
                EntityPresenter presenterInstance = Instantiate(m_entityPresenterPrefab, transform);
                presenterInstance.Initialize(turnContext.TypedModel, m_battle);

                m_currentPresenters.Add(turnContext.TypedModel, presenterInstance);
            }
        }

        private void HandleOnEntityDied(object obj, OnEntityDiedEventArgs args)
        {
            EntityPresenter presenter = m_currentPresenters[args.DiedEntity.Model];
            m_currentPresenters.Remove(args.DiedEntity.Model);

            // TODO: change to object pooling
            GameObject.Destroy(presenter.gameObject);
        }

        private void HandleOnBattleEnded(object obj, OnBattleEndedEventArgs args)
        {
            if (args.IsPlayerVictory)
            {
                foreach (EntityPresenter presenter in m_currentPresenters.Values)
                {
                    // TODO: change to object pooling
                    GameObject.Destroy(presenter.gameObject);
                }

                m_currentPresenters.Clear();
            }
        }
    }
}
