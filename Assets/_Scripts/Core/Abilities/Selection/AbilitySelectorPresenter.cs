using MagmaHeart.Abilities;
using MagmaHeart.Core.CombatSystem.Presenters;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Entities.Presenters;
using MagmaHeart.Core.Input.Mouse;
using MagmaHeart.Core.Presentation.UI;
using MagmaHeart.DungeonGeneration;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.Abilities.Selection
{
    public class AbilitySelectorPresenter : MonoBehaviour
    {
        [SerializeField] private Tilemap m_combatTilemap;
        [SerializeField] private TileBase m_validCombatTile;
        [SerializeField] private TileBase m_invalidCombatTile;

        [SerializeField] private Color m_allyOutlineColor = Color.green;
        [SerializeField] private Color m_enemyOutlineColor = Color.red;
        [SerializeField] private Color m_canAttackOutlineColor = new Color(1, 0.35f, 0.35f);

        [SerializeField] private EntityInfoUI m_entityInfoUI;
        [SerializeField] private TurnOrderPresenter m_turnOrderPresenter;

        private IGameWorld m_world;
        private EntityModel m_executor;

        private PlayerTurnController m_playerTurnController;
        private OnAbilitySelectedEventArgs m_currentSelection;
        private Entity m_currentEntitySelection;

        public void Initialize(IGameWorld world, EntityModel executor, PlayerTurnController playerTurnController)
        {
            m_world = world;
            m_executor = executor;
            m_playerTurnController = playerTurnController;

            m_playerTurnController.OnAbilitySelected += HandleOnAbilitySelected;
        }

        private void OnDisable()
        {
            m_playerTurnController.OnAbilitySelected -= HandleOnAbilitySelected;
        }

        private void HandleOnAbilitySelected(object _, OnAbilitySelectedEventArgs args)
        {
            if (m_currentSelection != null && m_currentSelection.HoverResult == args.HoverResult)
                return;
            
            Clear();

            m_currentSelection = args;
            Present(args);
        }

        private void Present(OnAbilitySelectedEventArgs selection)
        {
            bool hoversUI = selection.HoverResult.Type.HasFlag(HoverResultType.UI);
            if (hoversUI)
            {
                GameObject ui = selection.HoverResult.UI;

                if (ui != null && ui.TryGetComponent(out EntityPresenter entityPresenter))
                    if (entityPresenter.Entity != null)
                        PresentEntity(entityPresenter.Entity);

                return;
            }

            bool hoversTile = selection.HoverResult.Type.HasFlag(HoverResultType.Tile);
            if (hoversTile)
            {
                DungeonTile tile = selection.HoverResult.Tile;
                m_combatTilemap.SetTile(tile.Position.ToVector3Int(), m_invalidCombatTile);
            }

            bool hoversEntity = selection.HoverResult.Type.HasFlag(HoverResultType.Entity);
            if (hoversEntity)
                PresentEntity(selection.HoverResult.Entity);

            if (selection.Plan == null || !selection.Plan.IsLegal)
                return;

            // TODO: present ability
        }

        private void PresentEntity(Entity entity)
        {
            m_currentEntitySelection = entity;
            Color outlineColor = m_allyOutlineColor;

            if (m_world.AreEnemiesToEachOther(entity.Model.Id, m_executor.Id))
                outlineColor = m_enemyOutlineColor;

            entity.Outline.ApplyOutline(outlineColor);

            if (!entity.Model.IsPlayer)
                m_entityInfoUI.DisplayEntityInfo(entity.Model);
        }

        private void Clear()
        {
            m_combatTilemap.ClearAllTiles();
            m_entityInfoUI.Hide();

            if (m_currentEntitySelection != null)
            {
                m_currentEntitySelection.Outline.RemoveOutline();
                m_currentEntitySelection = null;
            }
        }
    }
}
