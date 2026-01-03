using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    [CreateAssetMenu(fileName = "new Location data", menuName = "Magma Heart Data/Location Data")]
    public class LocationData : ScriptableObject
    {
        [SerializeField] private string m_roomGenerationConfigFileName;

    }
}
