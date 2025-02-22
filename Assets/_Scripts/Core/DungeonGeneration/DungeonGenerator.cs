using MagmaHeart.Core.Dungeon;
using UnityEngine;

namespace MagmaHeart.Core
{
    public class DungeonGenerator : MonoBehaviour
    {
        [SerializeField] private LocationGenerator m_locationGenerator;

        private void Start() => Instantiate(m_locationGenerator);
    }
}