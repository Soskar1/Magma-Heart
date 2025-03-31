using System;

namespace MagmaHeart.Core.Entities
{
    public interface IAttacker
    {
        public Action OnAttackStarted { get; set; }
        public Action OnAttackEnded { get; set; }
        public void Attack();
    }
}