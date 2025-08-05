using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class MovementAction
    {
        public int MovementDistanceInTilesForOneEnergy { get; private set; } = 5;

        public void Move(Vector3Int tile)
        {

        }

        public int CalculateEnergyUsage(int distance) => Mathf.CeilToInt(distance / (float)MovementDistanceInTilesForOneEnergy);
    }
}