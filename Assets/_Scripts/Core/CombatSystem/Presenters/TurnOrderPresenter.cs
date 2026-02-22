using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Presenters;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem.Presenters
{
    public class TurnOrderPresenter : MonoBehaviour
    {
        [SerializeField] private EntityPresenter m_entityPresenterPrefab;
        private Dictionary<int, EntityPresenter> m_currentPresenters = new Dictionary<int, EntityPresenter>();

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
            foreach (Entity entity in args.TurnOrder)
            {
                EntityPresenter presenterInstance = Instantiate(m_entityPresenterPrefab, transform);
                presenterInstance.Initialize(entity, m_battle);

                m_currentPresenters.Add(entity.Model.Id, presenterInstance);
            }
        }

        private void HandleOnEntityDied(object obj, OnEntityDiedEventArgs args)
        {
            EntityPresenter presenter = m_currentPresenters[args.DiedEntity.Model.Id];
            m_currentPresenters.Remove(args.DiedEntity.Model.Id);

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
