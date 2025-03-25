using MagmaHeart.Core.Dungeon;
using UnityEngine;

namespace MagmaHeart.Core
{
    public class DungeonBootstrap : MonoBehaviour
    {
        [SerializeField] private Player m_player;
        [SerializeField] private LocationGenerator m_locationGenerator;

        private void Start() => BootScene();

        private void BootScene()
        {
            m_locationGenerator.GenerateLocation(Vector2Int.zero);

            Instantiate(m_player, transform.position, Quaternion.identity);
        }
    }
}