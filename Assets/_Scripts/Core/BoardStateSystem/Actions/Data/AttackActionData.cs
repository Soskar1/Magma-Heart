using MagmaHeart.AI.Actions;
using MagmaHeart.Core.BoardStateSystem.Actions.ArgumentCreators;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions.Data
{
    [CreateAssetMenu(fileName = "AttackActionData", menuName = "MagmaHeart/Actions/Attack Action")]
    public class AttackActionData : ActionData
    {
        [SerializeField] private int m_energyCost;
        [SerializeField] private int m_attackDistance;
        [SerializeField] private int m_attackDamage;

        public int EnergyCost => m_energyCost;
        public int AttackDistance => m_attackDistance;
        public int AttackDamage => m_attackDamage;

        public AttackActionData(int energyCost, int attackDistance, int attackDamage)
        {
            m_energyCost = energyCost;
            m_attackDistance = attackDistance;
            m_attackDamage = attackDamage;
        }

        public override ActionDefinition GetDefinition()
        {
            return new ActionDefinition(typeof(AttackAction), this, new AttackActionArgumentCreator(), new EnemyTargetSelector());
        }
    }
}
