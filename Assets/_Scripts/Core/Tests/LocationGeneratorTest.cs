using MagmaHeart.Core.Dungeon;
using UnityEngine;

namespace MagmaHeart.Core.Tests
{
    public class LocationGeneratorTest : MonoBehaviour
    {
        [SerializeField] private LocationGenerator m_locationGenerator;
        [SerializeField] private LocationRenderer m_renderer;

        public async void Generate()
        {
            m_renderer.Clear();
            Location location = await m_locationGenerator.GenerateLocation(Vector2Int.zero);
            StartCoroutine(m_renderer.DrawTiles(location.Tiles));
        }
    }
}