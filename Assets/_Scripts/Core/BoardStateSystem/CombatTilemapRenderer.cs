using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.BoardStateSystem
{
    [RequireComponent(typeof(Grid))]
    public class CombatTilemapRenderer : MonoBehaviour
    {
        [SerializeField] private Tilemap m_combatTilemap;
        [SerializeField] private TileBase m_combatTile;

        public Tilemap CombatTilemap => m_combatTilemap;

        public void DisplayCombatTile(CombatTile combatTile)
        {
            m_combatTilemap.SetTile(combatTile.Position, m_combatTile);
        }

        public void HideCombatTileAt(CombatTile combatTile)
        {
            m_combatTilemap.SetTile(combatTile.Position, null);
        }
    }
}