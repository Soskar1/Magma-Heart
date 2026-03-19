using System;
using System.Collections.Generic;
using MagmaHeart.Abilities.Effects;
using MagmaHeart.Core.Abilities.Effects;
using MagmaHeart.Core.Abilities.Effects.Presenters;
using MagmaHeart.Core.Abilities.Presentation;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.CombatSystem.Presenters;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Entities.Presenters;
using MagmaHeart.Core.Input.Mouse;
using MagmaHeart.Core.Presentation.UI;
using MagmaHeart.DungeonGeneration;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Selection
{
    public class AbilitySelectorPresenter : MonoBehaviour
    {
        [SerializeField] private CombatTilemapPresenter m_combatTilemapPresenter;
        [SerializeField] private EntityOutlinePresenter m_outlinePresenter;
        [SerializeField] private CursorPresenter m_cursorPresenter;

        [SerializeField] private EntityInfoUI m_entityInfoUI;
        [SerializeField] private TurnOrderPresenter m_turnOrderPresenter;
        [SerializeField] private EnergyPresenter m_energyPresenter;

        private GameWorld m_world;
        private EntityModel m_executor;

        private Battle m_battle;

        private PlayerTurnController m_playerTurnController;
        private Entity m_currentEntitySelection;

        private Dictionary<Type, IEffectPresenter> m_effectPresenters;
        private List<IEffectPresenter> m_currentActivePresenters;

        private bool m_presentSelection = false;

        public void Initialize(GameWorld world, EntityModel executor, PlayerTurnController playerTurnController, Battle battle)
        {
            m_world = world;
            m_executor = executor;
            m_playerTurnController = playerTurnController;
            m_currentActivePresenters = new List<IEffectPresenter>();
            m_battle = battle;

            m_effectPresenters = new Dictionary<Type, IEffectPresenter>
            {
                { typeof(DamageEffect), new DamageEffectPresenter(m_world, m_outlinePresenter, m_cursorPresenter) },
                { typeof(MoveEffect), new MoveEffectPresenter(m_combatTilemapPresenter) },
                { typeof(SpendResourceEffect), new SpendResourceEffectPresenter(m_energyPresenter) },
                { typeof(HealEffect), new HealEffectPresenter(m_world, m_outlinePresenter) },
                { typeof(KnockbackEffect), new KnockbackEffectPresenter(m_world, m_outlinePresenter, m_combatTilemapPresenter)  },
                { typeof(TeleportEffect), new TeleportEffectPresenter(m_combatTilemapPresenter, m_world, m_outlinePresenter) },
                { typeof(StunEffect), null }
            };

            m_playerTurnController.OnAbilitySelected += HandleOnAbilitySelected;
            m_battle.OnBattleStarted += HandleOnBattleStarted;
            m_battle.OnBattleEnded += HandleOnBattleEnded;
        }

        private void OnDisable()
        {
            m_playerTurnController.OnAbilitySelected -= HandleOnAbilitySelected;
            m_battle.OnBattleStarted -= HandleOnBattleStarted;
            m_battle.OnBattleEnded -= HandleOnBattleEnded;
        }

        private void HandleOnBattleStarted(object _, OnBattleStartedEventArgs __)
        {
            m_presentSelection = true;
        }

        private void HandleOnBattleEnded(object _, OnBattleEndedEventArgs __)
        {
            m_presentSelection = false;
            Clear();
        }

        private void HandleOnAbilitySelected(object _, OnAbilitySelectedEventArgs args)
        {
            if (!m_presentSelection)
                return;

            Clear();
            Present(args);
        }

        private void Present(OnAbilitySelectedEventArgs selection)
        {
            HoverResult hoverResult = selection.HoverResult;
            bool hoversEntity = selection.HoverResult.Type.HasFlag(HoverResultType.Entity);
            if (hoversEntity)
                PresentEntity(hoverResult.Entity);

            if (selection.Plan == null || !selection.Plan.IsLegal)
            {
                PresentDefaultSelection(selection.HoverResult);
                return;
            }

            foreach (AbilityEffect effect in selection.Plan.Effects)
            {
                Type type = effect.GetType();
                if (!m_effectPresenters.TryGetValue(type, out IEffectPresenter effectPresenter))
                {
                    Debug.LogWarning($"Presenter for {type} is not found!");
                    continue;
                }

                if (effectPresenter == null)
                {
                    continue;
                }

                effectPresenter.Present(effect);
                m_currentActivePresenters.Add(effectPresenter);
            }
        }

        private void PresentDefaultSelection(HoverResult hoverResult)
        {
            bool hoversUI = hoverResult.Type.HasFlag(HoverResultType.UI);
            if (hoversUI)
            {
                GameObject ui = hoverResult.UI;

                if (ui != null && ui.TryGetComponent(out EntityPresenter entityPresenter))
                    if (entityPresenter.Entity != null)
                        PresentEntity(entityPresenter.Entity);

                return;
            }

            bool hoversTile = hoverResult.Type.HasFlag(HoverResultType.Tile);
            if (hoversTile)
            {
                DungeonTile tile = hoverResult.Tile;
                m_combatTilemapPresenter.DisplayTile(tile.Position.ToVector3Int());
            }
        }

        private void PresentEntity(Entity entity)
        {
            if (!m_world.TryGetEntity(entity.Model.Id, out Entity _))
                return;

            m_currentEntitySelection = entity;

            m_outlinePresenter.OutlineEntity(entity, OutlineType.Ally);

            if (m_world.AreEnemiesToEachOther(entity.Model.Id, m_executor.Id))
                m_outlinePresenter.OutlineEntity(entity, OutlineType.Enemy);

            if (!entity.Model.IsPlayer)
                m_entityInfoUI.DisplayEntityInfo(entity.Model);
        }

        private void Clear()
        {
            m_combatTilemapPresenter.Clear();
            m_entityInfoUI.Hide();
            m_energyPresenter.DisplayCurrentEnergy();
            m_cursorPresenter.SetCursor(CursorType.Default);

            if (m_currentEntitySelection != null)
            {
                m_outlinePresenter.ClearOutline(m_currentEntitySelection);
                m_currentEntitySelection = null;
            }

            foreach (IEffectPresenter presenter in m_currentActivePresenters)
                presenter.Hide();

            m_currentActivePresenters.Clear();
        }
    }
}
