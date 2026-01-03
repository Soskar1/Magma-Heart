using MagmaHeart.AI.Actions;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions.Data
{
    [CreateAssetMenu(fileName = "MovementActionData", menuName = "Magma Heart Data/Actions/Movement Action")]
    public class MovementActionData : ActionData
    {
        [SerializeField] private int m_movementDistanceInTilesForOneEnergy = 2;

        public int MovementDistanceInTilesForOneEnergy => m_movementDistanceInTilesForOneEnergy;

        public MovementActionData(int movementDistanceInTilesForOneEnergy)
        {
            m_movementDistanceInTilesForOneEnergy = movementDistanceInTilesForOneEnergy;
        }

        public override ActionDefinition GetDefinition()
        {
            return new ActionDefinition(typeof(MovementAction), this, new MovementActionResolver());
        }
    }
}
