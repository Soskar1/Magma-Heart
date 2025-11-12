using MagmaHeart.Core.Entities.CombatSystem;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Entities.NonPlayableCharacters
{
    public class EnemyCombatController : CombatController
    {
        private readonly CombatAI m_ai;

        public EnemyCombatController(Entity entity, CombatAI ai) : base(entity)
        {
            m_ai = ai;
        }

        public override Task StartTurn()
        {
            return Task.CompletedTask;
        }
    }
}
