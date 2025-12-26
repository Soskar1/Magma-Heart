using MagmaHeart.AI.Actions;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions.Data
{
    [CreateAssetMenu(fileName = "AttackActionData", menuName = "MagmaHeart/Actions/Attack Action")]
    public class AttackActionData : ActionData
    {
        [SerializeField] private int m_energyCost;
        [SerializeField] private int m_attackDistance;
        [SerializeField] private int m_attackDamage;
        [SerializeField] private AttackType m_attackType;

        public int EnergyCost => m_energyCost;
        public int AttackDistance => m_attackDistance;
        public int AttackDamage => m_attackDamage;
        public AttackType AttackType => m_attackType;

        public AttackActionData(int energyCost, int attackDistance, int attackDamage, AttackType attackType)
        {
            m_energyCost = energyCost;
            m_attackDistance = attackDistance;
            m_attackDamage = attackDamage;
            m_attackType = attackType;
        }

        public override ActionDefinition GetDefinition()
        {
            IActionResolver resolver = m_attackType == AttackType.Melee ? new AttackActionResolver() : new RangedAttackActionResolver();
            return new ActionDefinition(typeof(AttackAction), this, resolver);
        }
    }
}
