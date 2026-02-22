using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.Abilities
{
    public class CombatTilemapPresenter : MonoBehaviour
    {
        [SerializeField] private Tilemap m_combatTilemap;
        [SerializeField] private TileBase m_validCombatTile;
        [SerializeField] private TileBase m_invalidCombatTile;

        public void DisplayTile(Vector3Int position, bool showValid = false)
        {
            TileBase tile = showValid ? m_validCombatTile : m_invalidCombatTile;
            m_combatTilemap.SetTile(position, tile);
        }

        public void Clear() => m_combatTilemap.ClearAllTiles();
    }
}
