using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class MovementAction
    {
        public int MovementDistanceInTilesForOneEnergy { get; private set; } = 5;

        public void Move(Vector3Int tile)
        {

        }

        public int CalculateEnergyUsage(int distance)
        {
            if (distance == 0)
                return 0;

            return distance / (MovementDistanceInTilesForOneEnergy + 1) + 1;
        }
    }
}